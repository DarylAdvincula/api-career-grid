using CG.DAL;
using CG.Models;
using CG.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Repositories.Implementations
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly AppDbContext _context;

        public ResumeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Resume?> GetByIdAsync(int id)
        {
            return await _context.Resumes.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Resume>> GetAllAsync()
        {
            return await _context.Resumes.AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Resume resume)
        {
            await _context.Resumes.AddAsync(resume);
        }

        public void Update(Resume resume)
        {
            _context.Resumes.Update(resume);
        }

        public void Delete(Resume resume)
        {
            _context.Resumes.Remove(resume);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}