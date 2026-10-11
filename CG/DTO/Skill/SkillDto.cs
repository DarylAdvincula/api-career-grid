namespace CG.DTO.Skill
{
    public class SkillDto
    {
        public int Id { get; set; }
        public int? JobClassificationId { get; set; }
        public int? JobSubClassificationId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}