using CG.DAL;
using CG.DTO.Auth;
using CG.DTO.UserAccount;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CG.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IPasswordService _passwordService;

        public AuthService(
            IUserAccountRepository userAccountRepository,
            IPasswordService passwordService
        )
        {
            _userAccountRepository = userAccountRepository;
            _passwordService = passwordService;
        }

        private static UserAccountDto ToDto(UserAccount userAccount)
        {
            return new UserAccountDto
            {
                FirstName = userAccount.FirstName,
                LastName = userAccount.LastName,
                Email = userAccount.Email,
            };
        }
        
        public async Task<UserAccountDto> RegisterAsApplicantAsync(UserAccountCreateDto request)
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

        public async Task LoginAsync(LoginDto request)
        {
            
        }

        public async Task LogoutAsync()
        {
            
        }
    }
}