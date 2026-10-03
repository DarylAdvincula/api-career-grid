using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CG.Models
{
    public enum ApprovalStatus
    {
        Draft,
        Pending,
        Published,
        Rejected
    }

    public enum JobType
    {
        Full_Time,
        Part_Time,
        Contractual_Temporary
    }

    public enum WorkEnvironment
    {
        On_Site,
        Hybrid,
        Remote
    }
    
    public class JobPosting
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CreatedByEmployerId { get; set; }
        public int? JobClassificationId { get; set; }
        public int? JobSubClassificationId { get; set; }
        public int? ApprovedByAdminId { get; set; }

        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [StringLength(5000)] // looks enough: https://www.blindtextgenerator.com/lorem-ipsum (5000 characters | 1 paragraph)
        public string RequirementsMarkdown { get; set; } = string.Empty;

        public JobType? JobType { get; set; }
        public WorkEnvironment? WorkEnvironment { get; set; }
        public string? Location { get; set; }

        [Column(TypeName = "money")]
        public decimal? MinSalary { get; set; }

        [Column(TypeName = "money")]
        public decimal? MaxSalary { get; set; }
        
        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Draft;
        public DateTime? ApprovedAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime PostedAt { get; init; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Company Company { get; set; } = null!;
        public UserAccount CreatedByEmployer { get; set; } = null!;
        public JobClassification? JobClassification { get; set; } = null!;
        public JobSubClassification? JobSubClassification { get; set; } = null!;
        public UserAccount? ApprovedByAdmin { get; set; } = null!;

        // +4 checks
        // [x] ApprovalStatus is in ApprovalStatus enum
        // [x] JobType is null or JobType is in JobType enum
        // [x] WorkEnvironment is null or WorkEnvironment in WorkEnvironment enum
        // [x] MaxSalary is null or MinSlary is null or MaxSalary is greater or equal to MinSalary

        // +3 index (non-clustered)
        // [x] CompanyId
        // [x] JobClassificationId
        // [x] JobSubClassificationId
    }
}