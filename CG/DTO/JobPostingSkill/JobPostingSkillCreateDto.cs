namespace CG.DTO.JobPostingSkill
{
    public class JobPostingSkillCreateDto
    {
        public int JobPostingId { get; set; }
        public int SkillId { get; set; }
        public bool IsRequired { get; set; }
    }
}