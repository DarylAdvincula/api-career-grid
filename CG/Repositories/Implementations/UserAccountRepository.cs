using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly AppDbContext _context;

        public UserAccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserAccount?> GetByIdAsync(int id)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<UserAccount?> GetByEmailAsync(string email)
        {
            return await _context.UserAccounts.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<IEnumerable<UserAccount>> GetAllAsync()
        {
            return await _context.UserAccounts.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(UserAccount user)
        {
            await _context.UserAccounts.AddAsync(user);
        }

        public void Update(UserAccount user)
        {
            _context.UserAccounts.Update(user);
        }

        public void Delete(UserAccount user)
        {
            _context.UserAccounts.Remove(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.UserAccounts.AnyAsync(x => x.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}