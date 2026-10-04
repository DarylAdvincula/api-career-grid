using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IWorkExperienceRepository
    {
        Task<WorkExperience?> GetByIdAsync(int id);
        Task<IEnumerable<WorkExperience>> GetAllAsync();
        Task AddAsync(WorkExperience workExperience);
        void Update(WorkExperience workExperience);
        void Delete(WorkExperience workExperience);
        Task SaveChangesAsync();
    }
}