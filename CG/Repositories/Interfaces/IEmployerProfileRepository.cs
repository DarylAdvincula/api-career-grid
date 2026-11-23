using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IEmployerProfileRepository
    {
        Task<EmployerProfile?> GetByIdAsync(int id);
        Task<EmployerProfile?> GetByUserAccountIdAsync(int userAccountId);
        Task<EmployerProfile?> GetByUserAccountAndCompanyIdAsync(int userAccountId, int companyId);
        Task<EmployerProfile?> GetCompanyOwnerByCompanyId(int companyId);
        Task<IEnumerable<EmployerProfile>> GetAllAsync();
        Task AddAsync(EmployerProfile employerProfile);
        void Update(EmployerProfile employerProfile);
        void Delete(EmployerProfile employerProfile);
        Task SaveChangesAsync();
    }
}