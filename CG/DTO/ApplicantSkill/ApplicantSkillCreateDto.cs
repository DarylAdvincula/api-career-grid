using CG.DTO.Skill;
using CG.Models;

namespace CG.DTO.ApplicantSkill
{
    public class ApplicantSkillCreateDto
    {
        public int? SkillId { get; set; }
        public int? YearsOfExperience { get; set; }
        public string ProficiencyLevel { get; set; } = string.Empty;
        public SkillCreateDto? Skill { get; set; }
    }
}