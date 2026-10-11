using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int id);
        Task<IEnumerable<Application>> GetAllByApplicantProfileIdAsync(int applicantProfileId);
        Task<IEnumerable<Application>> GetAllAsync();
        Task AddAsync(Application application);
        void Update(Application application);
        void Delete(Application application);
        Task SaveChangesAsync();
    }
}