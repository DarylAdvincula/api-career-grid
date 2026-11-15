namespace CG.DTO.Resume
{
    public class ResumeCreateDto
    {
        public IFormFile ResumeFile { get; set; } = null!;
        public bool IsPrimary { get; set; }
    }
}