using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class WorkExperienceRepository : IWorkExperienceRepository
    {
        private readonly AppDbContext _context;

        public WorkExperienceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<WorkExperience?> GetByIdAsync(int id)
        {
            return await _context.WorkExperiences.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<WorkExperience>> GetAllAsync()
        {
            return await _context.WorkExperiences.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(WorkExperience workExperience)
        {
            await _context.WorkExperiences.AddAsync(workExperience);
        }

        public void Update(WorkExperience workExperience)
        {
            _context.WorkExperiences.Update(workExperience);
        }

        public void Delete(WorkExperience workExperience)
        {
            _context.WorkExperiences.Remove(workExperience);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}