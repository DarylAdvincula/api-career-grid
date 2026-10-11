using CG.DTO.JobSubClassification;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class JobSubClassificationService : IJobSubClassificationService
    {
        private readonly IJobSubClassificationRepository _jobSubClassificationRepository;

        public JobSubClassificationService(IJobSubClassificationRepository jobSubClassificationRepository)
        {
            _jobSubClassificationRepository = jobSubClassificationRepository;
        }

        private async Task<JobSubClassification> GetByIdOrThrowAsync(int id)
        {
            var jobSubClassification = await _jobSubClassificationRepository.GetByIdAsync(id);

            if (jobSubClassification is null)
                throw new NotFoundException($"Job sub classification with id '{id}' does not exist.");
            
            return jobSubClassification;
        }

        private static JobSubClassificationDto ToDto(JobSubClassification jobSubClassification)
        {
            return new JobSubClassificationDto
            {
                Id = jobSubClassification.Id,
                JobClassificationId = jobSubClassification.JobClassificationId,
                Name = jobSubClassification.Name,
            };
        }

        public async Task<JobSubClassificationDto> GetByIdAsync(int id)
        {
            var jobSubClassification = await GetByIdOrThrowAsync(id);
            return ToDto(jobSubClassification);
        }

        public async Task<IEnumerable<JobSubClassificationDto>> GetAllAsync()
        {
            var jobSubClassifications = await _jobSubClassificationRepository.GetAllAsync();
            return jobSubClassifications.Select(ToDto);
        }

        public async Task<JobSubClassificationDto> CreateAsync(JobSubClassificationCreateDto request)
        {
            var newJobSubClassification = new JobSubClassification
            {
                JobClassificationId = request.JobClassificationId,
                Name = request.Name
            };

            await _jobSubClassificationRepository.AddAsync(newJobSubClassification);
            await _jobSubClassificationRepository.SaveChangesAsync();
            return ToDto(newJobSubClassification);
        }

        public async Task<JobSubClassificationDto> UpdateAsync(int id, JobSubClassificationUpdateDto request)
        {
            var jobSubClassification = await GetByIdOrThrowAsync(id);
            jobSubClassification.Name = request.Name;

            _jobSubClassificationRepository.Update(jobSubClassification);
            await _jobSubClassificationRepository.SaveChangesAsync();
            return ToDto(jobSubClassification);
        }

        public async Task DeleteAsync(int id)
        {
            var jobSubClassification = await GetByIdOrThrowAsync(id);

            _jobSubClassificationRepository.Delete(jobSubClassification);
            await _jobSubClassificationRepository.SaveChangesAsync();
        }
    }
}