using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface ISkillRepository
    {
        Task<Skill?> GetByIdAsync(int id);
        Task<Skill?> GetByNameAsync(string name);
        Task<IEnumerable<Skill>> GetAllAsync();
        Task AddAsync(Skill skill);
        void Update(Skill skill);
        void Delete(Skill skill);
        Task SaveChangesAsync();
    }
}