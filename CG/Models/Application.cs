using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public enum ApplicationStatus
    {
        Draft,
        Applied,
        Screening,
        Interviewing,
        Hired,
        Rejected
    }
    
    public class Application
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public int ApplicantProfileId { get; set; }
        public int? ResumeId { get; set; }
        public bool IsSubmitted { get; set; }
        public bool DraftStep { get; set; } = true;

        [StringLength(2000)]
        public string? CoverLetter { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Draft;
        public DateTime? SubmittedAt { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public JobPosting JobPosting { get; set; } = null!;
        public ApplicantProfile ApplicantProfile { get; set; } = null!;
        public Resume? Resume { get; set; }

        // +1 check
        // [x] ApplicationStatus is in ApplicationStatus enum

        // +1 constraint
        // [x] Unique (JobPostingId, ApplicantProfileId) together

        // +2 index (non-clustered)
        // [x] JobPostingID
        // [x] ApplicantProfileID
    }
}