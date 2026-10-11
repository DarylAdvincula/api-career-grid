namespace CG.DTO.Application
{
    public class ApplicationCreateDto
    {
        public int JobPostingId { get; set; }
        public int ApplicantProfileId { get; set; }
        public int? ResumeId { get; set; }
        public bool IsSubmitted { get; set; }
        public bool DraftStep { get; set; } // makes ApplicationStatus as Draft if true
        public string? CoverLetter { get; set; }
    }
}