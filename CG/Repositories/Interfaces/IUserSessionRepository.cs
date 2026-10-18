using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IUserSessionRepository
    {
        Task<UserSession?> GetByIdAsync(Guid id);
        Task<IEnumerable<UserSession>> GetAllByUserAccountIdAsync(int userAccountId, bool tracked);
        Task<IEnumerable<UserSession>> GetAllOtherActiveByUserAccountIdAsync(Guid id, int userAccountId, bool tracked);
        Task<IEnumerable<UserSession>> GetAllRevokedAsync(bool tracked);
        Task<IEnumerable<UserSession>> GetAllExpiredAsync(bool tracked);
        Task<IEnumerable<UserSession>> GetAllAsync();
        Task AddAsync(UserSession userSession);
        void Update(UserSession userSession);
        void Delete(UserSession userSession);
        Task SaveChangesAsync();
    }
}