using CG.DTO.Company;
using CG.Models;

namespace CG.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetByIdAsync(int id);

        Task<IEnumerable<CompanyDto>> GetAllAsync(
            string? search = null,
            VerificationStatus? verificationStatus = null
        );

        Task<CompanyDto> CreateAsync(CompanyCreateDto request);

        Task<CompanyDto> UpdateAsync(int id, CompanyUpdateDto request);

        Task<CompanyDto> ApproveAsync(int id);

        Task<CompanyDto> RejectAsync(int id);

        Task DeleteAsync(int id);
    }
}