using CG.DTO.Company;
using CG.DTO.EmployerProfile;

namespace CG.DTO.AccountProfile
{
    public class EmployerDto : AccountProfileDto
    {
        public EmployerProfileDto Profile { get; set; } = null!;
        public CompanyDto Company { get; set; } = null!;
    }
}