using CG.Models;

namespace CG.Services.Interfaces
{
    public enum PasswordVerifyResult
    {
        Failed,
        Success,
        SuccessRehashNeeded
    }
    
    public interface IPasswordService
    {
        string Hash(
            UserAccount user, 
            string password
        );

        PasswordVerifyResult Verify(
            UserAccount user, 
            string password
        );
    }
}