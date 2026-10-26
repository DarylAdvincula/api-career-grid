using CG.DTO.UserAccount;
using CG.Models;

namespace CG.DTO.Company
{
    public class CompanyJoinDto : UserAccountCreateDto
    {
        public int CompanyId { get; set; }
        public CompanyRole CompanyRole { get; set; }
    }
}
