using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IApplicantSkillRepository
    {
        Task<ApplicantSkill?> GetByIdAsync(int id);
        Task<IEnumerable<ApplicantSkill>> GetAllAsync();
        Task AddAsync(ApplicantSkill applicantSkill);
        void Update(ApplicantSkill applicantSkill);
        void Delete(ApplicantSkill applicantSkill);
        Task SaveChangesAsync();
    }
}