using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int id);
        Task<List<Application>> GetAllByApplicantProfileIdAsync(int applicantProfileId, ApplicationStatus? applicationStatus = null);
        Task<List<Application>> GetAllByCompanyAndJobPostingIdAsync(
            int companyId,
            int jobPostingId
        );
        Task AddAsync(Application application);
        void Update(Application application);
        void Delete(Application application);
        Task SaveChangesAsync();
    }
}