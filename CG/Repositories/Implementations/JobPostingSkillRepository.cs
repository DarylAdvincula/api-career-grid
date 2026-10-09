using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class JobPostingSkillRepository : IJobPostingSkillRepository
    {
        private readonly AppDbContext _context;

        public JobPostingSkillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<JobPostingSkill?> GetByIdAsync(int id)
        {
            return await _context.JobPostingSkills.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<JobPostingSkill>> GetAllByJobPostingIdAsync(int jobPostingId)
        {
            return await _context.JobPostingSkills.Where(x => x.JobPostingId == jobPostingId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<JobPostingSkill>> GetAllAsync()
        {
            return await _context.JobPostingSkills.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(JobPostingSkill jobPostingSkill)
        {
            await _context.JobPostingSkills.AddAsync(jobPostingSkill);
        }

        public void Update(JobPostingSkill jobPostingSkill)
        {
            _context.JobPostingSkills.Update(jobPostingSkill);
        }

        public void Delete(JobPostingSkill jobPostingSkill)
        {
            _context.JobPostingSkills.Remove(jobPostingSkill);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}