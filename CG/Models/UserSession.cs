namespace CG.Models
{
    public class UserSession
    {
        public Guid Id { get; set; }
        public int UserAccountId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreateAt { get; set; }
        public UserAccount UserAccount { get; set; } = null!;
    }
}