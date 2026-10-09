using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class ApplicantSkillRepository : IApplicantSkillRepository
    {
        private readonly AppDbContext _context;

        public ApplicantSkillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicantSkill?> GetByIdAsync(int id)
        {
            return await _context.ApplicantSkills.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ApplicantSkill>> GetAllByApplicantProfileIdAsync(int applicantProfileId)
        {
            return await _context.ApplicantSkills.Where(x => x.ApplicantProfileId == applicantProfileId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ApplicantSkill>> GetAllAsync()
        {
            return await _context.ApplicantSkills.ToListAsync();
        }

        public async Task AddAsync(ApplicantSkill applicantSkill)
        {
            await _context.ApplicantSkills.AddAsync(applicantSkill);
        }

        public void Update(ApplicantSkill applicantSkill)
        {
            _context.ApplicantSkills.Update(applicantSkill);
        }

        public void Delete(ApplicantSkill applicantSkill)
        {
            _context.ApplicantSkills.Remove(applicantSkill);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}