using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(int id);
        Task<IEnumerable<Company>> GetAllAsync();
        Task AddAsync(Company company);
        void Update(Company company);
        void Delete(Company company);
        Task SaveChangesAsync();
    }
}