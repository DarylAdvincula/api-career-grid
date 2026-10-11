using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class ApplicantProfileRepository : IApplicantProfileRepository
    {
        private readonly AppDbContext _context;

        public ApplicantProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicantProfile?> GetByIdAsync(int id)
        {
            return await _context.ApplicantProfiles.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ApplicantProfile?> GetByUserAccountIdAsync(int userAccountId)
        {
            return await _context.ApplicantProfiles.FirstOrDefaultAsync(x => x.UserAccountId == userAccountId);
        }

        public async Task<IEnumerable<ApplicantProfile>> GetAllAsync()
        {
            return await _context.ApplicantProfiles.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(ApplicantProfile applicantProfile)
        {
            await _context.ApplicantProfiles.AddAsync(applicantProfile);
        }

        public void Update(ApplicantProfile applicantProfile)
        {
            _context.ApplicantProfiles.Update(applicantProfile);
        }

        public void Delete(ApplicantProfile applicantProfile)
        {
            _context.ApplicantProfiles.Remove(applicantProfile);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}