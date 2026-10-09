using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class SkillRepository : ISkillRepository
    {
        public readonly AppDbContext _context;

        public SkillRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Skill?> GetByIdAsync(int id)
        {
            return await _context.Skills.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Skill?> GetByNameAsync(string name)
        {
            return await _context.Skills.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task<IEnumerable<Skill>> GetAllAsync()
        {
            return await _context.Skills.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Skill skill)
        {
            await _context.Skills.AddAsync(skill);
        }

        public void Update(Skill skill)
        {
            _context.Skills.Update(skill);
        }

        public void Delete(Skill skill)
        {
            _context.Skills.Remove(skill);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}