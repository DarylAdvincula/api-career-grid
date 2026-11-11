using CG.DTO.AccountProfile;
using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IStaffService
    {
        Task<List<EmployerDto>> GetAll(
            string? search = null,
            CompanyRole? role = null
        );
        Task<EmployerDto> Approve(int id);
        Task<EmployerDto> Reject(int id);
    }
}