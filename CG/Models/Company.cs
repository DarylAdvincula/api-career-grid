using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public enum VerificationStatus
    {
        Pending,
        Approved,
        Rejected
    }
    
    public class Company
    {
        public int Id { get; set; }

        // admin id (from UserAccounts)
        public int? ApprovedByAdminId { get; set; }

        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1500)] // instead of nvarchar(max)
        public string Description { get; set; } = string.Empty;

        [StringLength(2083)]
        public string WebsiteUrl { get; set; } = string.Empty;

        [StringLength(2083)]
        public string LogoUrl { get; set; } = string.Empty;

        [StringLength(10)]
        public string Code { get; set; } = string.Empty;

        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;
        public DateTime? ApprovedAt { get; set; }
        public DateTime CreatedAt { get; init; } = DateTime.Now;

        // admin
        public UserAccount ApprovedByAdmin { get; set; } = null!;
        
        // +1 check
        // [x] VerificationStatus is in VerificationStatuses enum
    }
}