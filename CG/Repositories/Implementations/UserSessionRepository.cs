using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly AppDbContext _context;

        public UserSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserSession?> GetByIdAsync(Guid id)
        {
            return await _context.UserSessions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<UserSession>> GetAllByUserAccountIdAsync(int userAccountId, bool tracked)
        {
            var userSessionsQuery = _context.UserSessions.AsQueryable()
                .Where(x => x.UserAccountId == userAccountId);

            if (tracked)
                return await userSessionsQuery.ToListAsync();

            return await userSessionsQuery.AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSession>> GetAllOtherActiveByUserAccountIdAsync(int userAccountId, bool tracked)
        {
            var now = DateTime.Now;
            var userSessionsQuery = _context.UserSessions.AsQueryable()
                .Where(x =>
                (
                    x.UserAccountId == userAccountId &&
                    x.ExpiresAt > now
                ));

            if (tracked)
                return await userSessionsQuery.ToListAsync();

            return await userSessionsQuery.AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSession>> GetAllRevokedByUserAccountIdAsync(int userAccountId, bool tracked)
        {
            var userSessionsQuery = _context.UserSessions.AsQueryable()
                .Where(x => x.UserAccountId == userAccountId);

            if (tracked)
                return await userSessionsQuery.ToListAsync();

            return await userSessionsQuery.AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSession>> GetAllExpiredByUserAccountIdAsync(int userAccountId, bool tracked)
        {
            var now = DateTime.Now;
            var userSessionsQuery = _context.UserSessions.AsQueryable()
                .Where(x =>
                (
                    x.UserAccountId == userAccountId &&
                    x.ExpiresAt <= now
                ));

            if (tracked)
                return await userSessionsQuery.ToListAsync();

            return await userSessionsQuery.AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<UserSession>> GetAllAsync()
        {
            return await _context.UserSessions.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(UserSession userSession)
        {
            await _context.UserSessions.AddAsync(userSession);
        }

        public void Update(UserSession userSession)
        {
            _context.UserSessions.Update(userSession);
        }

        public void Delete(UserSession userSession)
        {
            _context.UserSessions.Remove(userSession);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}