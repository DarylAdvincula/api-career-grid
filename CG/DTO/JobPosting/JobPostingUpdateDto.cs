using CG.Models;

namespace CG.DTO.JobPosting
{
    public class JobPostingUpdateDto
    {
        public int? JobClassificationId { get; set; }
        public int? JobSubClassificationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RequirementsMarkdown { get; set; } = string.Empty;
        public JobType? JobType { get; set; }
        public WorkEnvironment? WorkEnvironment { get; set; }
        public string? Location { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
    }
}