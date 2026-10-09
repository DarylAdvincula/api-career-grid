using CG.Models;
using CG.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CG.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<UserAccount> _hasher = new();

        public string Hash(UserAccount user, string password)
        {
            return _hasher.HashPassword(user, password);
        }

        public PasswordVerifyResult Verify(UserAccount user, string password)
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result switch
            {
                PasswordVerificationResult.Success => PasswordVerifyResult.Success,
                PasswordVerificationResult.SuccessRehashNeeded => PasswordVerifyResult.SuccessRehashNeeded,
                _ => PasswordVerifyResult.Failed
            };
        }
    }
}