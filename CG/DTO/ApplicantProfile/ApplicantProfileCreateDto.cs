using CG.DTO.ApplicantSkill;
using CG.DTO.Resume;

namespace CG.DTO.ApplicantProfile
{
    public class ApplicantProfileCreateDto
    {
        public string? Headline { get; set; }
        public string? HomeLocation { get; set; }
        public string? Bio { get; set; }
        public List<ApplicantSkillCreateDto> Skills { get; set; } = [];
        public List<ResumeCreateDto> Resumes { get; set; } = [];
    }
}