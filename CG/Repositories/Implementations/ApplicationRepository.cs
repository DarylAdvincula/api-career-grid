using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly AppDbContext _context;

        public ApplicationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(int id)
        {
            return await _context.Applications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Application>> GetAllByApplicantProfileIdAsync(int applicantProfileId)
        {
            return await _context.Applications.Where(x => x.ApplicantProfileId == applicantProfileId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetAllAsync()
        {
            return await _context.Applications.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Application application)
        {
            await _context.Applications.AddAsync(application);
        }

        public void Update(Application application)
        {
            _context.Applications.Update(application);
        }

        public void Delete(Application application)
        {
            _context.Applications.Remove(application);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}