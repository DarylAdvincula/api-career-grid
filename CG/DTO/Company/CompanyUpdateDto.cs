using CG.Models;

namespace CG.DTO.Company
{
    public class CompanyUpdateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public VerificationStatus VerificationStatus { get; set; }
    }
}