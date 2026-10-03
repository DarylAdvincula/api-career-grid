namespace CG.Models
{
    public enum ProficiencyLevel
    {
        Beginner,
        Intermediate,
        Advanced,
        Expert
    }
    
    public class ApplicantSkill
    {
        public int Id { get; set; }
        public int ApplicantProfileId { get; set; }
        public int SkillId { get; set; }

        public ProficiencyLevel? ProficiencyLevel { get; set; }
        public int? YearsOfExperience { get; set; }
        public ApplicantProfile ApplicantProfile { get; set; } = null!;
        public Skill Skill { get; set; } = null!;
        
        // +1 check
        // [x] ProficiencyLevel is null or ProficiencyLevel is in ProficiencyLevel enum
        
        // +1 constraint
        // [x] Unique (ApplicationProfileId, SkillId) together

        // +1 index (non-clustered)
        // [x] SkillId
    }
}