namespace CG.DTO.Application
{
    public class ApplicationUpdateDto
    {
        public int? ResumeId { get; set; }
        public bool DraftStep { get; set; } // makes ApplicationStatus as Draft if true
        public string? CoverLetter { get; set; }
    }
}