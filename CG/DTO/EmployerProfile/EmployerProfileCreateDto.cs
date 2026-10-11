using CG.Models;

namespace CG.DTO.EmployerProfile
{
    public class EmployerProfileCreateDto
    {
        public int UserAccountId { get; set; }
        public int CompanyId { get; set; }
        public CompanyRole CompanyRole { get; set; }
    }
}