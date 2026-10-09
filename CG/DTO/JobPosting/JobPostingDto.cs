using CG.Models;

namespace CG.DTO.JobPosting
{
    public class JobPostingDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int CreatedByEmployerId { get; set; }
        public int? JobClassificationId { get; set; }
        public int? JobSubClassificationId { get; set; }
        public int? ApprovedByAdminId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RequirementsMarkdown { get; set; } = string.Empty;
        public JobType? JobType { get; set; }
        public WorkEnvironment? WorkEnvironment { get; set; }
        public string? Location { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public bool IsActive { get; set; }
        public DateTime PostedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}