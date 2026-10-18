namespace CG.DTO.UserSession
{
    public class UserSessionCreateDto
    {
        public Guid Id { get; init; } = Guid.NewGuid();
        public int UserAccountId { get; set; }
    }
}