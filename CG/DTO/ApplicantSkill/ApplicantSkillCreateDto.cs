using CG.DTO.Skill;

namespace CG.DTO.ApplicantSkill
{
    public class ApplicantSkillCreateDto
    {
        public int? SkillId { get; set; }
        public SkillCreateDto? Skill { get; set; }
    }
}