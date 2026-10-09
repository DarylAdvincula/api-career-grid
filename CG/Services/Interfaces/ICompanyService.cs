using CG.DTO.Company;

namespace CG.Services.Interfaces
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetByIdAsync(int id);

        Task<IEnumerable<CompanyDto>> GetAllAsync();

        Task<CompanyDto> CreateAsync(CompanyCreateDto request);

        Task<CompanyDto> UpdateAsync(int id, CompanyUpdateDto request);

        Task DeleteAsync(int id);
    }
}