using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IUserSessionRepository
    {
        Task<UserSession?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserSession>> GetAllByUserAccountIdAsync(int userAccountId, bool tracked);
        Task<IEnumerable<UserSession>> GetAllOtherActiveByUserAccountIdAsync(int userAccountId, bool tracked);
        Task<IEnumerable<UserSession>> GetAllRevokedByUserAccountIdAsync(int userAccountId, bool tracked);
        Task<IEnumerable<UserSession>> GetAllExpiredByUserAccountIdAsync(int userAccountId, bool tracked);
        Task<IEnumerable<UserSession>> GetAllAsync();
        Task AddAsync(UserSession userSession);
        void Update(UserSession userSession);
        void Delete(UserSession userSession);
        Task SaveChangesAsync();
    }
}