using CG.DTO.UserAccount;

namespace CG.DTO.Company
{
    public class CompanyJoinDto : UserAccountCreateDto
    {
        public string CompanyCode { get; set; } = string.Empty;
        public string CompanyRole { get; set; } = string.Empty;
    }
}
