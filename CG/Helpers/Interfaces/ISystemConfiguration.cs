namespace CG.Helpers.Interfaces
{
    public interface ISystemConfiguration
    {
        void CheckCompleteness();
        public T GetValue<T>(string key, string feature);
        public string AdminFirstName { get; }
        public string AdminLastName { get; }
        public string AdminEmail { get; }
        public string AdminPassword { get; }
        public string AppBaseUrl { get; }
        public string AppFrontendUrl { get; }
        public string DatabaseConnectionString { get; }
        public int LifeSpanSession { get; }
        public int LifeSpanEmailVerificationCode { get; }
        public string GmailSmtpHost { get; }
        public int GmailSmtpPort { get; }
        public string GmailAddress { get; }
        public string GmailAppPassword { get; }
        public string GmailAppName { get; }
    }
}