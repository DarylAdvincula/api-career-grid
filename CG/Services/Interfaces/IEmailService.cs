using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string recipient, string subject, string htmlBody);
        Task SendEmailVerificationAsync(UserAccount userAccount);
        Task SendEmailVerifiedAsync(UserAccount userAccount);
    }
}