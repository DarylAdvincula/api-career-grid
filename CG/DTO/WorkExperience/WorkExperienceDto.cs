namespace CG.DTO.WorkExperience
{
    public class WorkExperienceDto
    {
        public int Id { get; set; }
        public int ApplicantProfileId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentRole { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}