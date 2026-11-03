using CG.DTO.JobClassification;
using CG.DTO.JobSubClassification;

namespace CG.DTO.Skill
{
    public class SkillCreateDto
    {
        public int? JobClassificationId { get; set; } = null;
        public int? JobSubClassificationId { get; set; } = null;
        public string Name { get; set; } = string.Empty;
    }
}