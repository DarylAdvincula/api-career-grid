using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public class Resume
    {
        public int Id { get; set; }
        public int ApplicantProfileId { get; set; }

        [StringLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [StringLength(255)]
        public string StoredFileName { get; set; } = string.Empty;

        [StringLength(500)]
        public string FilePath { get; set; } = string.Empty;

        [StringLength(10)]
        public string FileExtension { get; set; } = string.Empty;

        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;

        public int FileSizeInBytes { get; set; }
        public bool IsPrimary { get; set; } // resume version that's flagged as primary
        public DateTime UploadedAt { get; init; } = DateTime.Now;
        public ApplicantProfile ApplicantProfile { get; set; } = null!;

        // +1 constraint | +2 index (non-clustered)
        // [x] ApplicantProfileId
        // [x] Each Applicant should only have 1 primary resume
    }
}