using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly AppDbContext _context;

        public JobPostingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobPosting?> GetByIdAsync(int id)
        {
            return await _context.JobPostings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<JobPosting>> GetAllAsync()
        {
            return await _context.JobPostings.ToListAsync();
        }

        public async Task AddAsync(JobPosting jobPosting)
        {
            await _context.JobPostings.AddAsync(jobPosting);
        }

        public void Update(JobPosting jobPosting)
        {
            _context.JobPostings.Update(jobPosting);
        }

        public void Delete(JobPosting jobPosting)
        {
            _context.JobPostings.Remove(jobPosting);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}