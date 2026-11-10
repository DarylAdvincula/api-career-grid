using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IBaseEmployerService
    {
        Task<EmployerProfile> GetCurrentProfileAsync();
        Task<Company> GetCurrentCompanyAsync();
    }
}