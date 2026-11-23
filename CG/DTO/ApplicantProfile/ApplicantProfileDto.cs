using CG.DTO.ApplicantSkill;
using CG.DTO.Resume;
using CG.DTO.UserAccount;

namespace CG.DTO.ApplicantProfile
{
    public class ApplicantProfileDto
    {
        public int Id { get; set; }
        public int UserAccountId { get; set; }
        public string? Headline { get; set; }
        public string? HomeLocation { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ApplicantSkillDto> Skills { get; set; } = [];
        public List<ResumeDto> Resumes { get; set; } = [];
        public UserAccountDto? UserAccount { get; set; }
    }
}