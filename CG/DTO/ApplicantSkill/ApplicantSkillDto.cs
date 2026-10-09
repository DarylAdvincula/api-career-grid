using CG.Models;

namespace CG.DTO.ApplicantSkill
{
    public class ApplicantSkillDto
    {
        public int Id { get; set; }
        public int ApplicantProfileId { get; set; }
        public int SkillId { get; set; }
        public ProficiencyLevel? ProficiencyLevel { get; set; }
        public int? YearsOfExperience { get; set; }
    }
}