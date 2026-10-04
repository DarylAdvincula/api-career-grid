using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class JobSubClassificationRepository : IJobSubClassificationRepository
    {
        private readonly AppDbContext _context;

        public JobSubClassificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobSubClassification?> GetByIdAsync(int id)
        {
            return await _context.JobSubClassifications.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<JobSubClassification>> GetAllAsync()
        {
            return await _context.JobSubClassifications.ToListAsync();
        }

        public async Task AddAsync(JobSubClassification jobSubClassification)
        {
            await _context.JobSubClassifications.AddAsync(jobSubClassification);
        }

        public void Update(JobSubClassification jobSubClassification)
        {
            _context.JobSubClassifications.Update(jobSubClassification);
        }

        public void Delete(JobSubClassification jobSubClassification)
        {
            _context.JobSubClassifications.Remove(jobSubClassification);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}