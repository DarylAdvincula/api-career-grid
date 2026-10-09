using CG.DTO.JobClassification;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class JobClassificationService : IJobClassificationService
    {
        private readonly IJobClassificationRepository _jobClassificationRepository;

        public JobClassificationService(IJobClassificationRepository jobClassificationRepository)
        {
            _jobClassificationRepository = jobClassificationRepository;
        }

        private async Task<JobClassification> GetByIdOrThrowAsync(int id)
        {
            var jobClassification = await _jobClassificationRepository.GetByIdAsync(id);

            if (jobClassification is null)
                throw new NotFoundException($"Job classification with id '{id}' does not exist.");
            
            return jobClassification;
        }

        private static JobClassificationDto ToDto(JobClassification jobClassification)
        {
            return new JobClassificationDto
            {
                Id = jobClassification.Id,
                Name = jobClassification.Name,
                Description = jobClassification.Description,
                CreatedAt = jobClassification.CreatedAt
            };
        }

        public async Task<JobClassificationDto> GetByIdAsync(int id)
        {
            var jobClassification = await GetByIdOrThrowAsync(id);
            return ToDto(jobClassification);
        }

        public async Task<IEnumerable<JobClassificationDto>> GetAllAsync()
        {
            var jobClassifications = await _jobClassificationRepository.GetAllAsync();
            return jobClassifications.Select(ToDto);
        }

        public async Task<JobClassificationDto> CreateAsync(JobClassificationCreateDto request)
        {
            var existingJobClassification = await _jobClassificationRepository.GetByNameAsync(request.Name);

            if (existingJobClassification is not null)
                throw new ConflictException($"Job classification with name '{request.Name}' already exist.");
            
            var newJobClassification = new JobClassification
            {
                Name = request.Name,
                Description = request.Description
            };

            await _jobClassificationRepository.AddAsync(newJobClassification);
            await _jobClassificationRepository.SaveChangesAsync();
            return ToDto(newJobClassification);
        }

        public async Task<JobClassificationDto> UpdateAsync(int id, JobClassificationUpdateDto request)
        {
            var jobClassification = await GetByIdOrThrowAsync(id);
            jobClassification.Name = request.Name;
            jobClassification.Description = request.Description;

            _jobClassificationRepository.Update(jobClassification);
            await _jobClassificationRepository.SaveChangesAsync();
            return ToDto(jobClassification);
        }

        public async Task DeleteAsync(int id)
        {
            var jobClassification = await GetByIdOrThrowAsync(id);

            _jobClassificationRepository.Delete(jobClassification);
            await _jobClassificationRepository.SaveChangesAsync();
        }
    }
}