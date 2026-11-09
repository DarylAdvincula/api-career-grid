using CG.DTO.UserAccount;

namespace CG.DTO.Company
{
    public class CompanyJoinDto : UserAccountCreateDto
    {
        public int CompanyId { get; set; }
        public string CompanyRole { get; set; } = string.Empty;
    }
}
