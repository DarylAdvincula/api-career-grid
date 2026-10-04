using CG.DAL;
using CG.Models;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class JobPostingSkillRepository
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

        public async Task<IEnumerable<JobPostingSkill>> GetAllAsync()
        {
            return await _context.JobPostingSkills.ToListAsync();
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