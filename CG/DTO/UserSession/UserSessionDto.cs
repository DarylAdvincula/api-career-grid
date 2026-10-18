namespace CG.DTO.UserSession
{
    public class UserSessionDto
    {
        public Guid Id { get; set; }
        public int UserAccountId { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public DateTime? RevokedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}