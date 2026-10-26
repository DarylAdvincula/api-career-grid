using System.Security.Claims;
using CG.DAL;
using CG.DTO.AccountProfile;
using CG.DTO.Company;
using CG.DTO.UserAccount;
using CG.DTO.UserSession;
using CG.Exceptions;
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
        private readonly ICompanyService _companyService;
        private readonly IEmployerProfileRepository _employerProfileRepository;
        private readonly IEmployerProfileService _employerProfileService;
        private readonly IApplicantProfileService _applicantProfileService;
        private readonly IPasswordService _passwordService;
        private readonly IUserSessionService _userSessionService;

        public AuthService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            ICompanyRepository companyRepository,
            ICompanyService companyService,
            IEmployerProfileRepository employerProfileRepository,
            IEmployerProfileService employerProfileService,
            IApplicantProfileService applicantProfileService,
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
            _companyService = companyService;
            _employerProfileRepository = employerProfileRepository;
            _employerProfileService = employerProfileService;
            _applicantProfileService = applicantProfileService;
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
                var newUserAccount = new UserAccount
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Role = Role.Applicant
                };

                newUserAccount.PasswordHash = _passwordService.Hash(newUserAccount, request.Password);

                await _userAccountRepository.AddAsync(newUserAccount);
                await _userAccountRepository.SaveChangesAsync();
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
            
            try
            {
                var newUserAccount = new UserAccount
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Role = Role.Employer
                };

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
        
        public async Task<UserAccountDto> RegisterAsStaff(CompanyJoinDto request)
        {
            if (request.CompanyRole == CompanyRole.Owner)
                throw new BadRequestException("You cannot request to join into a company as an owner.");
            
            using var transaction = await _context.Database.BeginTransactionAsync();
            
            try
            {
                var newUserAccount = new UserAccount
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Role = Role.Employer
                };

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
            
            // Bypass for now
            // if (!user.IsVerified)
            //     throw new ForbiddenException("Please verify your email first.");
            
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
    }
}