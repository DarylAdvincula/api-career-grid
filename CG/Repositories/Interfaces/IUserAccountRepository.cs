using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<UserAccount?> GetByIdAsync(int id);
        Task<UserAccount?> GetByEmailAsync(string email);
        Task<IEnumerable<UserAccount>> GetAllAsync();

        Task AddAsync(UserAccount userAccount);
        void Update(UserAccount userAccount);
        void Delete(UserAccount userAccount);

        Task<bool> EmailExistsAsync(string email);
        Task SaveChangesAsync();
    }
}