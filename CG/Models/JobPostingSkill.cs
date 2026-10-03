namespace CG.Models
{
    public class JobPostingSkill
    {
        public int Id { get; set; }
        public int JobPostingId { get; set; }
        public int SkillId { get; set; }
        public bool IsRequired { get; set; } = true;
        public JobPosting JobPosting { get; set; } = null!;
        public Skill Skill { get; set; } = null!;

        // +1 constraint
        // [x] Unique (JobPostingId, SkillId) together

        // +1 index
        // [x] SkillId
    }
}