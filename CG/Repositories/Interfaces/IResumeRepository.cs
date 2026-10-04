using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IResumeRepository
    {
        Task<Resume?> GetByIdAsync(int id);
        Task<IEnumerable<Resume>> GetAllAsync();
        Task AddAsync(Resume resume);
        void Update(Resume resume);
        void Delete(Resume resume);
        Task SaveChangesAsync();
    }
}