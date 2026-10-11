namespace CG.DTO.Resume
{
    public class ResumeUpdateDto
    {
        public IFormFile ResumeFile { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }
}