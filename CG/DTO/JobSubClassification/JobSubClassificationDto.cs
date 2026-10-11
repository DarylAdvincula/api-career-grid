namespace CG.DTO.JobSubClassification
{
    public class JobSubClassificationDto
    {
        public int Id { get; set; }
        public int JobClassificationId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}