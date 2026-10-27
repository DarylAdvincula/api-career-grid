using CG.Helpers.Interfaces;

namespace CG.Helpers.Implementations
{
    public class SystemConfiguration : ISystemConfiguration
    {
        private readonly IConfiguration _configuration;

        public SystemConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public T GetValue<T>(string key, string feature)
        {
            var value = _configuration.GetValue<T>(key);

            if (value is null)
                throw new InvalidOperationException($"[{feature}] '{key}' is not configured.");

            return value;
        }

        public string AdminFirstName => GetValue<string>("Seed:AdminFirstName", "Startup");
        public string AdminLastName => GetValue<string>("Seed:AdminLastName", "Startup");
        public string AdminEmail => GetValue<string>("Seed:AdminEmail", "Startup");
        public string AdminPassword => GetValue<string>("Seed:AdminPassword", "Startup");
        public string AppBaseUrl => GetValue<string>("App:BaseUrl", "App");
        public string AppFrontendUrl => GetValue<string>("App:FrontendUrl", "App");
        public string DatabaseConnectionString => GetValue<string>("Database:ConnectionString", "Database");
        public int LifeSpanSession => GetValue<int>("LifeSpan:Hours:Session", "Auth");
        public int LifeSpanEmailVerificationCode => GetValue<int>("LifeSpan:Minutes:EmailVerificationCode", "Auth");
        public string GmailSmtpHost => GetValue<string>("Gmail:SmtpHost", "Mailing");
        public int GmailSmtpPort => GetValue<int>("Gmail:SmtpPort", "Mailing");
        public string GmailAddress => GetValue<string>("Gmail:Username", "Mailing");
        public string GmailAppPassword => GetValue<string>("Gmail:AppPassword", "Mailing");
        public string GmailAppName => GetValue<string>("Gmail:AppName", "Mailing");

        public void CheckCompleteness()
        {
            var adminFirstName = AdminFirstName;
            var adminLastName = AdminLastName;
            var adminEmail = AdminEmail;
            var adminPassword = AdminPassword;
            var appBaseUrl = AppBaseUrl;
            var appFrontendUrl = AppFrontendUrl;
            var databaseConnectionString = DatabaseConnectionString;
            var lifeSpanSession = LifeSpanSession;
            var lifeSpanEmailVerificationCode = LifeSpanEmailVerificationCode;
            var gmailSmtpHost = GmailSmtpHost;
            var gmailSmtpPort = GmailSmtpPort;
            var gmailAddress = GmailAddress;
            var gmailAppPassword = GmailAppPassword;
            var gmailAppName = GmailAppName;
        }
    }
}