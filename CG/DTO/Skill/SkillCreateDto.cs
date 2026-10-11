namespace CG.DTO.Skill
{
    public class SkillCreateDto
    {
        public int? JobClassificationId { get; set; }
        public int? JobSubClassificationId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}