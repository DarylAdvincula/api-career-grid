using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IBaseApplicantService
    {
        Task<ApplicantProfile?> GetOptionalCurrentProfileAsync();
        Task<ApplicantProfile> GetRequiredCurrentProfileAsync();
    }
}