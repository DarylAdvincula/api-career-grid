using System.ComponentModel.DataAnnotations;

namespace CG.Models
{
    public class WorkExperience
    {
        public int Id { get; set; }
        public int ApplicantProfileId { get; set; }

        [StringLength(100)]
        public string JobTitle { get; set; } = string.Empty;

        [StringLength(150)]
        public string Company { get; set; } = string.Empty;

        [StringLength(150)]
        public string Location { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentRole { get; set; }

        [StringLength(1500)]
        public string Description { get; set; } = string.Empty;

        public ApplicantProfile ApplicantProfile { get; set; } = null!;
        // +2 checks
        // [x] End date is null or end date must be later than or equal to start date
        // [x] Is current role can only be true if end date is null or is current role is false

        // +1 index (non-clustered)
        // [x] ApplicantProfileId
    }
}