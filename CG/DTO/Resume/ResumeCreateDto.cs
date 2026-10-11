namespace CG.DTO.Resume
{
    public class ResumeCreateDto
    {
        public IFormFile ResumeFile { get; set; } = null!;
        public int ApplicantProfileId { get; set; }
        public bool IsPrimary { get; set; }
    }
}