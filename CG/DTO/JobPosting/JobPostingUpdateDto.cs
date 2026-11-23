using CG.Models;

namespace CG.DTO.JobPosting
{
    public class JobPostingUpdateDto
    {
        public string Title { get; set; } = string.Empty;
        public string RequirementsMarkdown { get; set; } = string.Empty;
        public string? JobType { get; set; }
        public string? WorkEnvironment { get; set; }
        public string? Location { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
    }
}