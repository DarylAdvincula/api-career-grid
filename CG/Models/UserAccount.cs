using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public enum Role
    {
        Applicant,
        Employer,
        Admin
    }
    
    public class UserAccount
    {
        public int Id { get; set; }

        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public Role Role { get; set; }
        public bool IsVerified { get; set; }
        public string? VerificationCode { get; set; }
        public DateTime? VerificationExpiry { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // +2 checks
        // [x] Role is in Roles enum
        // [x] Email format is valid

        // +1 constraint | +1 index (non-clustered)
        // [x] Email (unique)
    }
}