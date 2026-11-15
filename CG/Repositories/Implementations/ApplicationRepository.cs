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

        public async Task<List<Application>> GetAllByApplicantProfileIdAsync(int applicantProfileId, ApplicationStatus? applicationStatus = null)
        {
            var applicationsQuery = _context.Applications.AsQueryable()
                .Where(x => x.ApplicantProfileId == applicantProfileId);

            if (applicationStatus is not null)
                applicationsQuery = applicationsQuery.Where(x => x.ApplicationStatus == applicationStatus);

            return await applicationsQuery
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Application>> GetAllByCompanyAndJobPostingIdAsync(int companyId, int jobPostingId)
        {
            return await _context.Applications.Where(x =>
            (
                x.JobPosting.CompanyId == companyId &&
                x.JobPostingId == jobPostingId
            ))
                .Include(x => x.JobPosting)
                .AsNoTracking()
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