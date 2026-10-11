using CG.DAL;
using CG.DTO.Resume;
using CG.Exceptions;
using CG.Helpers.Implementations;
using CG.Helpers.Interfaces;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class ResumeService : IResumeService
    {
        private readonly AppDbContext _context;
        private readonly IResumeRepository _resumeRepository;
        private readonly string _uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Resumes");

        public ResumeService(
            AppDbContext context,
            IResumeRepository resumeRepository
        )
        {
            _context = context;
            _resumeRepository = resumeRepository;

            if (!Directory.Exists(_uploadFolder))
                Directory.CreateDirectory(_uploadFolder);
        }

        private async Task<Resume> GetByIdOrThrowAsync(int id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);

            if (resume is null)
                throw new NotFoundException($"Resume with id '{id}' does not exist.");
            
            return resume;
        }

        private static ResumeDto ToDto(Resume resume)
        {
            return new ResumeDto
            {
                Id = resume.Id,
                ApplicantProfileId = resume.ApplicantProfileId,
                OriginalFileName = resume.OriginalFileName,
                StoredFileName = resume.StoredFileName,
                FilePath = resume.FilePath,
                FileExtension = resume.FileExtension,
                ContentType = resume.ContentType,
                FileSizeInBytes = resume.FileSizeInBytes,
                IsPrimary = resume.IsPrimary,
                UploadedAt = resume.UploadedAt
            };
        }

        public async Task<ResumeDto> GetByIdAsync(int id)
        {
            var resume = await GetByIdOrThrowAsync(id);
            return ToDto(resume);
        }

        public async Task<IEnumerable<ResumeDto>> GetAllByApplicantProfileIdAsync(int id)
        {
            var resumes = await _resumeRepository.GetAllByApplicantProfileIdAsync(id);
            return resumes.Select(ToDto);
        }

        public async Task<IEnumerable<ResumeDto>> GetAllAsync()
        {
            var resumes = await _resumeRepository.GetAllAsync();
            return resumes.Select(ToDto);
        }

        public async Task<ResumeDto> CreateAsync(ResumeCreateDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
           
            try
            {
                var uploadFileValidator = new UploadFileValidator(request.ResumeFile);

                uploadFileValidator.EnsureValidOrThow();

                var uploadFileMetadataExtractor = new UploadFileMetadataExtractor(request.ResumeFile);

                using var stream = new FileStream(
                    uploadFileMetadataExtractor.GetFilePath(_uploadFolder),
                    FileMode.Create
                );

                await request.ResumeFile.CopyToAsync(stream);

                var newResume = new Resume
                {
                    ApplicantProfileId = request.ApplicantProfileId,
                    OriginalFileName = uploadFileMetadataExtractor.GetFileName(),
                    StoredFileName = uploadFileMetadataExtractor.GetUniqueFileName(),
                    FilePath = uploadFileMetadataExtractor.GetFilePath(_uploadFolder),
                    FileExtension = uploadFileMetadataExtractor.GetExtension(),
                    ContentType = uploadFileMetadataExtractor.GetContentType(),
                    FileSizeInBytes = uploadFileMetadataExtractor.GetSizeInBytes(),
                    IsPrimary = request.IsPrimary
                };

                if (request.IsPrimary)
                {
                    var primaryResume = await _resumeRepository.GetPrimaryByApplicantProfileId(request.ApplicantProfileId);

                    if (primaryResume is not null)
                    {
                        primaryResume.IsPrimary = false;
                        
                        _resumeRepository.Update(primaryResume);
                        await _resumeRepository.SaveChangesAsync();
                    }
                }

                await _resumeRepository.AddAsync(newResume);
                await _resumeRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                return ToDto(newResume);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ResumeDto> UpdateAsync(int id, ResumeUpdateDto request)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var resume = await GetByIdOrThrowAsync(id);
                
                if (File.Exists(resume.FilePath))
                    File.Delete(resume.FilePath);

                var uploadFileValidator = new UploadFileValidator(request.ResumeFile);

                uploadFileValidator.EnsureValidOrThow();

                var uploadFileMetadataExtractor = new UploadFileMetadataExtractor(request.ResumeFile);

                using var stream = new FileStream(
                    uploadFileMetadataExtractor.GetFilePath(_uploadFolder),
                    FileMode.Create
                );

                resume.OriginalFileName = uploadFileMetadataExtractor.GetFileName();
                resume.StoredFileName = uploadFileMetadataExtractor.GetUniqueFileName();
                resume.FilePath = uploadFileMetadataExtractor.GetFilePath(_uploadFolder);
                resume.FileExtension = uploadFileMetadataExtractor.GetExtension();
                resume.ContentType = uploadFileMetadataExtractor.GetContentType();
                resume.FileSizeInBytes = uploadFileMetadataExtractor.GetSizeInBytes();
                resume.IsPrimary = request.IsPrimary;

                if (request.IsPrimary)
                {
                    var primaryResume = await _resumeRepository.GetPrimaryByApplicantProfileId(resume.ApplicantProfileId);

                    if (primaryResume is not null)
                    {
                        primaryResume.IsPrimary = false;
                        
                        _resumeRepository.Update(primaryResume);
                        await _resumeRepository.SaveChangesAsync();
                    }
                }

                await _resumeRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                return ToDto(resume);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var resume = await GetByIdOrThrowAsync(id);
            
            _resumeRepository.Delete(resume);
            await _resumeRepository.SaveChangesAsync();
        }
    }
}