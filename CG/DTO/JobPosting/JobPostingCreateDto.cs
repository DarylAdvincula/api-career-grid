namespace CG.DTO.JobPosting
{
    public class JobPostingCreateDto
    {
        public int? JobClassificationId { get; set; }
        public int? JobSubClassificationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string RequirementsMarkdown { get; set; } = string.Empty;
        public string? JobType { get; set; }
        public string? WorkEnvironment { get; set; }
        public string? Location { get; set; }
        public decimal? MinSalary { get; set; }
        public decimal? MaxSalary { get; set; }
        public bool IsDraft { get; set; }
    }
}