using CG.DTO.ApplicantSkill;

namespace CG.DTO.ApplicantProfile
{
    public class ApplicantProfileCreateDto
    {
        public int UserAccountId { get; set; }
        public string? Headline { get; set; }
        public string? HomeLocation { get; set; }
        public string? Bio { get; set; }
        public List<ApplicantSkillCreateDto> Skills { get; set; } = [];
    }
}