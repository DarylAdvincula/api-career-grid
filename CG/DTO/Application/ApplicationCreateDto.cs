namespace CG.DTO.Application
{
    public class ApplicationCreateDto
    {
        public int JobPostingId { get; set; }
        public int ResumeId { get; set; }
        public bool IsDraft { get; set; }
        public string? CoverLetter { get; set; }
    }
}