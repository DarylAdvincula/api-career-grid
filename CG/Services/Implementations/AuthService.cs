using System.Security.Claims;
using CG.DAL;
using CG.DTO.AccountProfile;
using CG.DTO.Company;
using CG.DTO.UserAccount;
using CG.DTO.UserSession;
using CG.Exceptions;
using CG.Helpers.Interfaces;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CG.Services.Implementations
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ICompanyRepository _companyRepository;
        private readonly ISystemConfiguration _systemConfiguration;
        private readonly ICompanyService _companyService;
        private readonly IEmployerProfileRepository _employerProfileRepository;
        private readonly IEmployerProfileService _employerProfileService;
        private readonly IApplicantProfileService _applicantProfileService;
        private readonly IEmailService _emailService;
        private readonly IPasswordService _passwordService;
        private readonly IUserSessionService _userSessionService;

        public AuthService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            ICompanyRepository companyRepository,
            ISystemConfiguration systemConfiguration,
            ICompanyService companyService,
            IEmployerProfileRepository employerProfileRepository,
            IEmployerProfileService employerProfileService,
            IApplicantProfileService applicantProfileService,
            IEmailService emailService,
            IPasswordService passwordService,
            IUserSessionService userSessionService
        ) : base(
            httpContextAccessor, 
            userAccountRepository
        )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userAccountRepository = userAccountRepository;
            _companyRepository = companyRepository;
            _systemConfiguration = systemConfiguration;
            _companyService = companyService;
            _employerProfileRepository = employerProfileRepository;
            _employerProfileService = employerProfileService;
            _applicantProfileService = applicantProfileService;
            _emailService = emailService;
            _passwordService = passwordService;
            _userSessionService = userSessionService;
        }

        private async Task Authenticate(
            int userAccountId,
            string email,
            Role role,
            Guid userSessionId
        )
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, userAccountId.ToString()),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, role.ToString()),
                new("session_id", userSessionId.ToString())
            };
            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var httpContext = _httpContextAccessor?.HttpContext;

            if (httpContext is null)
                throw new InvalidOperationException("No active HTTP context.");
            
            await  httpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );
        }

        private UserAccount ToModel(UserAccountCreateDto userAccountCreateDto, Role role)
        {
            return new UserAccount
            {
                FirstName = userAccountCreateDto.FirstName,
                LastName = userAccountCreateDto.LastName,
                Email = userAccountCreateDto.Email,
                Role = role,
                VerificationExpiry = DateTime.Now.AddMinutes(_systemConfiguration.LifeSpanEmailVerificationCode),
                VerificationCode = Guid.NewGuid().ToString(),
            };
        }

        private static UserAccountDto ToDto(UserAccount userAccount)
        {
            return new UserAccountDto
            {
                Id = userAccount.Id,
                Email = userAccount.Email,
                FirstName = userAccount.FirstName,
                LastName = userAccount.LastName,
                Role = userAccount.Role,
                IsVerified = userAccount.IsVerified,
                CreatedAt = userAccount.CreatedAt,
                UpdatedAt = userAccount.UpdatedAt
            };
        }

        public async Task<AccountProfileDto> MeAsync()
        {
            var currentUserAccount = await GetCurrentUser();
            
            if (currentUserAccount.Role == Role.Admin)
            {
                return new AdminDto
                {
                    Account = ToDto(currentUserAccount)
                };
            }
            else if (currentUserAccount.Role == Role.Employer)
            {
                var employerProfile = await _employerProfileService.GetByUserAccountIdAsync(currentUserAccount.Id);
                var company = await _companyService.GetByIdAsync(employerProfile.CompanyId);
                return new EmployerDto
                {
                    Account = ToDto(currentUserAccount),
                    Profile = employerProfile,
                    Company = company
                };
            }
            else
            {
                var applicantProfile = await _applicantProfileService.GetByUserAccountIdAsync(currentUserAccount.Id);
                return new ApplicantDto
                {
                    Account = ToDto(currentUserAccount),
                    Profile = applicantProfile
                };
            }
        }
        
        public async Task<UserAccountDto> RegisterAsApplicant(UserAccountCreateDto request)
        {
            try
            {
                var newUserAccount = ToModel(request, Role.Applicant);
                newUserAccount.PasswordHash = _passwordService.Hash(newUserAccount, request.Password);

                await _userAccountRepository.AddAsync(newUserAccount);
                await _userAccountRepository.SaveChangesAsync();
                await _emailService.SendEmailVerificationAsync(newUserAccount);
                return ToDto(newUserAccount);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 } sql)
            {
                throw new ConflictException("Email is already registered.");
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<UserAccountDto> RegisterAsOwner(CompanyCreateDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            var newUserAccount = ToModel(request, Role.Employer);
            
            try
            {
                newUserAccount.PasswordHash = _passwordService.Hash(newUserAccount, request.Password);

                await _userAccountRepository.AddAsync(newUserAccount);
                await _userAccountRepository.SaveChangesAsync();

                var newCompany = new Company
                {
                    Name = request.Name,
                    Description = request.Description,
                    WebsiteUrl = request.WebsiteUrl,
                    LogoUrl = request.LogoUrl
                };

                await _companyRepository.AddAsync(newCompany);
                await _companyRepository.SaveChangesAsync();

                var newUserProfile = new EmployerProfile
                {
                    UserAccountId = newUserAccount.Id,
                    CompanyId = newCompany.Id,
                    CompanyRole = CompanyRole.Owner
                };

                await _employerProfileRepository.AddAsync(newUserProfile);
                await _employerProfileRepository.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 } sql)
            {
                await transaction.RollbackAsync();
                throw new ConflictException("Email is already registered.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }

            await _emailService.SendEmailVerificationAsync(newUserAccount);
            return ToDto(newUserAccount);
        }
        
        public async Task<UserAccountDto> RegisterAsStaff(CompanyJoinDto request)
        {
            if (request.CompanyRole == CompanyRole.Owner)
                throw new BadRequestException("You cannot request to join into a company as an owner.");
            
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var newUserAccount = ToModel(request, Role.Employer);
                newUserAccount.PasswordHash = _passwordService.Hash(newUserAccount, request.Password);

                await _userAccountRepository.AddAsync(newUserAccount);
                await _userAccountRepository.SaveChangesAsync();

                var newUserProfile = new EmployerProfile
                {
                    UserAccountId = newUserAccount.Id,
                    CompanyId = request.CompanyId,
                    CompanyRole = request.CompanyRole
                };

                await _employerProfileRepository.AddAsync(newUserProfile);
                await _employerProfileRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                await _emailService.SendEmailVerificationAsync(newUserAccount);
                return ToDto(newUserAccount);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 } sql)
            {
                await transaction.RollbackAsync();
                throw new ConflictException("Email is already registered.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task LoginAsync(UserAccountLoginDto request)
        {
            var user = await _userAccountRepository.GetByEmailAsync(request.Email);

            if (user is null)
                throw new UnauthorizedException("Invalid email or password.");
            
            if (!user.IsVerified)
                throw new ForbiddenException("Please verify your email first.");
            
            var passwordVerificationResult = _passwordService.Verify(user, request.Password);
            
            if (passwordVerificationResult == PasswordVerifyResult.Failed)
                throw new UnauthorizedException("Invalid email or password");
            
            if (passwordVerificationResult == PasswordVerifyResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordService.Hash(user, request.Password);
                
                _userAccountRepository.Update(user);
                await _userAccountRepository.SaveChangesAsync();
            }

            var newUserSession = await _userSessionService.CreateAsync(new UserSessionCreateDto { UserAccountId = user.Id });
            
            await Authenticate(
                user.Id,
                user.Email,
                user.Role,
                newUserSession.Id
            );
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
                throw new InvalidOperationException("No active HTTP context.");

            var user = httpContext.User;

            if (Guid.TryParse(user.FindFirstValue("session_id"), out var sessionId))
                await _userSessionService.RevokeAsync(sessionId);
            
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task VerifyEmailAsync(string verificationCode)
        {
            var userAccount = await _userAccountRepository.GetByVerificationCodeAsync(verificationCode);

            if (userAccount is null)
                throw new BadRequestException("Invalid verification code.");
            
            // mail a new email verification if the existing code was already expired
            if (DateTime.Now >= userAccount.VerificationExpiry)
            {
                userAccount.VerificationCode = Guid.NewGuid().ToString();
                userAccount.VerificationExpiry = DateTime.Now.AddMinutes(_systemConfiguration.LifeSpanEmailVerificationCode);

                _userAccountRepository.Update(userAccount);
                await _userAccountRepository.SaveChangesAsync();
                await _emailService.SendEmailVerificationAsync(userAccount);
            }
            else
            {
                userAccount.IsVerified = true;
                userAccount.VerificationCode = null;
                userAccount.VerificationExpiry = null;

                _userAccountRepository.Update(userAccount);
                await _userAccountRepository.SaveChangesAsync();
                await _emailService.SendEmailVerifiedAsync(userAccount);
            }
        }
    }
}