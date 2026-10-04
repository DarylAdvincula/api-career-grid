using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IApplicantProfileRepository
    {
        Task<ApplicantProfile?> GetByIdAsync(int id);
        Task<ApplicantProfile?> GetByUserAccountIdAsync(int userAccountId);
        Task<IEnumerable<ApplicantProfile>> GetAllAsync();
        Task AddAsync(ApplicantProfile applicantProfile);
        void Update(ApplicantProfile applicantProfile);
        void Delete(ApplicantProfile applicantProfile);
        Task SaveChangesAsync();
    }
}