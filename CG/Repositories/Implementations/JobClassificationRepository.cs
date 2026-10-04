using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class JobClassificationRepository : IJobClassificationRepository
    {
        private readonly AppDbContext _context;

        public JobClassificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobClassification?> GetByIdAsync(int id)
        {
            return await _context.JobClassifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<JobClassification>> GetAllAsync()
        {
            return await _context.JobClassifications.ToListAsync();
        }

        public async Task AddAsync(JobClassification jobClassification)
        {
            await _context.JobClassifications.AddAsync(jobClassification);
        }

        public void Update(JobClassification jobClassification)
        {
            _context.JobClassifications.Update(jobClassification);
        }

        public void Delete(JobClassification jobClassification)
        {
            _context.JobClassifications.Remove(jobClassification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}