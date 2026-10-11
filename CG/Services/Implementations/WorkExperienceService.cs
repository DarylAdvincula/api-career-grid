using CG.DTO.WorkExperience;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class WorkExperienceService : IWorkExperienceService
    {
        private readonly IWorkExperienceRepository _workExperienceRepository;

        public WorkExperienceService(IWorkExperienceRepository workExperienceRepository)
        {
            _workExperienceRepository = workExperienceRepository;
        }

        private async Task<WorkExperience> GetByIdOrThrowAsync(int id)
        {
            var workExperience = await _workExperienceRepository.GetByIdAsync(id);

            if (workExperience is null)
                throw new NotFoundException($"Work experience with id '{id}' does not exist.");
            
            return workExperience;
        }

        private static WorkExperienceDto ToDto(WorkExperience workExperience)
        {
            return new WorkExperienceDto
            {
                Id = workExperience.Id,
                ApplicantProfileId = workExperience.ApplicantProfileId,
                JobTitle = workExperience.JobTitle,
                Company = workExperience.Company,
                Location = workExperience.Location,
                StartDate = workExperience.StartDate,
                EndDate = workExperience.EndDate,
                IsCurrentRole = workExperience.IsCurrentRole,
                Description = workExperience.Description
            };
        }

        public async Task<WorkExperienceDto> GetByIdAsync(int id)
        {
            var workExperience = await GetByIdOrThrowAsync(id);
            return ToDto(workExperience);
        }

        public async Task<IEnumerable<WorkExperienceDto>> GetAllByApplicantProfileIdAsync(int id)
        {
            var workExperiences = await _workExperienceRepository.GetAllByApplicantProfileIdAsync(id);
            return workExperiences.Select(ToDto);
        }

        public async Task<IEnumerable<WorkExperienceDto>> GetAllAsync()
        {
            var workExperiences = await _workExperienceRepository.GetAllAsync();
            return workExperiences.Select(ToDto);
        }

        public async Task<WorkExperienceDto> CreateAsync(WorkExperienceCreateDto request)
        {
            var newWorkExperience = new WorkExperience
            {
                ApplicantProfileId = request.ApplicantProfileId,
                JobTitle = request.JobTitle,
                Company = request.Company,
                Location = request.Location,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsCurrentRole = request.IsCurrentRole,
                Description = request.Description
            };

            if (newWorkExperience.IsCurrentRole && newWorkExperience.EndDate is not null)
                throw new BadRequestException("A current role cannot have an end date. Please remove the end date or uncheck 'Is Current Role'.");
            
            if (!newWorkExperience.IsCurrentRole && newWorkExperience.EndDate is null)
                throw new BadRequestException($"An end date is required if '{newWorkExperience.JobTitle}' is not your current role.");
            
            await _workExperienceRepository.AddAsync(newWorkExperience);
            await _workExperienceRepository.SaveChangesAsync();
            return ToDto(newWorkExperience);
        }

        public async Task<WorkExperienceDto> UpdateAsync(int id, WorkExperienceUpdateDto request)
        {
            var workExperience = await GetByIdOrThrowAsync(id);
            workExperience.JobTitle = request.JobTitle;
            workExperience.Company = request.Company;
            workExperience.Location = request.Location;
            workExperience.StartDate = request.StartDate;
            workExperience.EndDate = request.EndDate;
            workExperience.IsCurrentRole = request.IsCurrentRole;
            workExperience.Description = request.Description;

            if (workExperience.IsCurrentRole && workExperience.EndDate is not null)
                throw new BadRequestException("A current role cannot have an end date. Please remove the end date or uncheck 'Is Current Role'.");
            
            if (!workExperience.IsCurrentRole && workExperience.EndDate is null)
                throw new BadRequestException($"An end date is required if '{workExperience.JobTitle}' is not your current role.");
            
            _workExperienceRepository.Update(workExperience);
            await _workExperienceRepository.SaveChangesAsync();
            return ToDto(workExperience);
        }

        public async Task DeleteAsync(int id)
        {
            var workExperience = await GetByIdOrThrowAsync(id);

            _workExperienceRepository.Delete(workExperience);
            await _workExperienceRepository.SaveChangesAsync();
        }
    }
}