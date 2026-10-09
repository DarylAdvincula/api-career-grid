using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class EmployerProfileRepository : IEmployerProfileRepository
    {
        private readonly AppDbContext _context;

        public EmployerProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EmployerProfile?> GetByIdAsync(int id)
        {
            return await _context.EmployerProfiles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<EmployerProfile?> GetByUserAccountIdAsync(int userAccountId)
        {
            return await _context.EmployerProfiles.FirstOrDefaultAsync(x => x.UserAccountId == userAccountId);
        }

        public async Task<IEnumerable<EmployerProfile>> GetAllAsync()
        {
            return await _context.EmployerProfiles.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(EmployerProfile employerProfile)
        {
            await _context.EmployerProfiles.AddAsync(employerProfile);
        }

        public void Update(EmployerProfile employerProfile)
        {
            _context.EmployerProfiles.Update(employerProfile);
        }

        public void Delete(EmployerProfile employerProfile)
        {
            _context.EmployerProfiles.Remove(employerProfile);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}