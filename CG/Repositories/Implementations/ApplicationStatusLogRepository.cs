using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class ApplicationStatusLogRepository : IApplicationStatusLogRepository
    {
        private readonly AppDbContext _context;
        
        public ApplicationStatusLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationStatusLog?> GetByIdAsync(int id)
        {
            return await _context.ApplicationStatusLogs.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ApplicationStatusLog?> GetByApplicationIdAsync(int applicationId)
        {
            return await _context.ApplicationStatusLogs.FirstOrDefaultAsync(x => x.ApplicationId == applicationId);
        }

        public async Task<IEnumerable<ApplicationStatusLog>> GetAllByApplicantProfileIdAsync(int applicantProfileId)
        {
            return await _context.ApplicationStatusLogs.Include(x => x.Application)
                .Where(x => x.Application.ApplicantProfileId == applicantProfileId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicationStatusLog>> GetAllAsync()
        {
            return await _context.ApplicationStatusLogs.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(ApplicationStatusLog applicationStatusLog)
        {
            await _context.ApplicationStatusLogs.AddAsync(applicationStatusLog);
        }

        public void Update(ApplicationStatusLog applicationStatusLog)
        {
            _context.ApplicationStatusLogs.Update(applicationStatusLog);
        }

        public void Delete(ApplicationStatusLog applicationStatusLog)
        {
            _context.ApplicationStatusLogs.Remove(applicationStatusLog);
        }
        
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}