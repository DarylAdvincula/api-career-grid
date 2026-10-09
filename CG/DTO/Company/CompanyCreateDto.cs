namespace CG.DTO.Company
{
    public class CompanyCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string WebsiteUrl { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}