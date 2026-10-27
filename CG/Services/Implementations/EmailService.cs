using CG.Helpers.Interfaces;
using CG.Models;
using CG.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CG.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly ISystemConfiguration _systemConfiguration;
        private readonly IEmailTemplateService _emailTemplateService;

        public EmailService(
            ISystemConfiguration systemConfiguration,
            IEmailTemplateService emailTemplateService
        )
        {
            _systemConfiguration = systemConfiguration;
            _emailTemplateService = emailTemplateService;
        }
        
        public async Task SendAsync(string to, string subject, string htmlBody)
        {
            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _systemConfiguration.GmailAppName,
                    _systemConfiguration.GmailAddress
                )
            );

            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = new BodyBuilder
            {
                HtmlBody = htmlBody
            }.ToMessageBody();

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _systemConfiguration.GmailSmtpHost,
                _systemConfiguration.GmailSmtpPort,
                SecureSocketOptions.StartTls
            );
            await client.AuthenticateAsync(
                _systemConfiguration.GmailAddress,
                _systemConfiguration.GmailAppPassword
            );
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendEmailVerificationAsync(UserAccount userAccount)
        {
            var body = await _emailTemplateService.RenderAsync("Verification", new Dictionary<string, string>
            {
                ["Name"] = $"{userAccount.FirstName}",
                ["VerificationLink"] = $"{_systemConfiguration.AppBaseUrl}/auth/verify/email?code={userAccount.VerificationCode}"
            });

            await SendAsync(userAccount.Email, "CareerGrid Email Verification", body);
        }

        public async Task SendEmailVerifiedAsync(UserAccount userAccount)
        {
            var body = await _emailTemplateService.RenderAsync("Verified", new Dictionary<string, string>
            {
                ["Name"] = $"{userAccount.FirstName}",
                ["LoginLink"] = $"{_systemConfiguration.AppFrontendUrl}/auth/login"
            });

            await SendAsync(userAccount.Email, "Email Verification Success!", body);
        }
    }
}