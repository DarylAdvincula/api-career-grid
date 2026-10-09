namespace CG.DTO.UserAccount
{
    public class UserAccountCreateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string RepeatedPassword { get; set; } = string.Empty;
    }
}