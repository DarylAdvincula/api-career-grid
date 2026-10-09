using CG.Models;

namespace CG.DTO.Application
{
    public class ApplicationDto
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public int ApplicantProfileId { get; set; }
        public int? ResumeId { get; set; }
        public bool IsSubmitted { get; set; }
        public bool DraftStep { get; set; }
        public string? CoverLetter { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime? SubmittedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}