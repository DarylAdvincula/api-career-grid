using CG.DTO.Company;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class CompanyService : BaseService, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(
            ICompanyRepository companyRepository,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository
        ) : base(
            httpContextAccessor,
            userAccountRepository
        )
        {
            _companyRepository = companyRepository;
        }

        private async Task<Company> GetByIdOrThrowAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company is null)
                throw new NotFoundException($"Company with id '{id}' does not exist.");
            
            return company;
        }

        private static CompanyDto ToDto(Company company)
        {
            return new CompanyDto
            {
                Id = company.Id,
                ApprovedByAdminId = company.ApprovedByAdminId,
                Name = company.Name,
                Description = company.Description,
                WebsiteUrl = company.WebsiteUrl,
                LogoUrl = company.LogoUrl,
                VerificationStatus = company.VerificationStatus,
                ApprovedAt = company.ApprovedAt,
                CreatedAt = company.CreatedAt,
            };
        }

        public async Task<CompanyDto> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            
            if (company is null)
                throw new NotFoundException($"Company with id '{id}' does not exist.");
            
            return ToDto(company);
        }

        public async Task<IEnumerable<CompanyDto>> GetAllAsync(
            string? search = null,
            VerificationStatus? verificationStatus = null
        )
        {
            var companies = await _companyRepository.GetAllAsync(search, verificationStatus);
            return companies.Select(ToDto);
        }

        public async Task<CompanyDto> CreateAsync(CompanyCreateDto request)
        {
            var newCompany = new Company
            {
                Name = request.Name,
                Description = request.Description,
                WebsiteUrl = request.WebsiteUrl,
                LogoUrl = request.LogoUrl,
                VerificationStatus = VerificationStatus.Pending,
            };

            await _companyRepository.AddAsync(newCompany);
            await _companyRepository.SaveChangesAsync();
            return ToDto(newCompany);
        }

        public async Task<CompanyDto> UpdateAsync(int id, CompanyUpdateDto request)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company is null)
                throw new NotFoundException($"Company with id '{id}' does not exist.");

            company.Name = request.Name;
            company.Description = request.Description;
            company.WebsiteUrl = request.WebsiteUrl;
            company.LogoUrl = request.LogoUrl;
            company.VerificationStatus = request.VerificationStatus;

            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync();
            return ToDto(company);
        }

        public async Task<CompanyDto> ApproveAsync(int id)
        {
            var company = await GetByIdOrThrowAsync(id);

            if (company.VerificationStatus != VerificationStatus.Pending)
                throw new BadRequestException("You can only approve companies that are pending for approval.");
            
            var currentUser = await GetCurrentUserAsync();

            company.VerificationStatus = VerificationStatus.Approved;
            company.ApprovedByAdminId = currentUser.Id;
            company.ApprovedAt = DateTime.Now;

            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync();
            return ToDto(company);
        }

        public async Task<CompanyDto> RejectAsync(int id)
        {
            var company = await GetByIdOrThrowAsync(id);

            if (company.VerificationStatus != VerificationStatus.Pending)
                throw new BadRequestException("You can only reject companies that are pending for approval.");
            
            var currentUser = await GetCurrentUserAsync();

            company.VerificationStatus = VerificationStatus.Rejected;
            company.ApprovedByAdminId = currentUser.Id;
            company.ApprovedAt = DateTime.Now;

            _companyRepository.Update(company);
            await _companyRepository.SaveChangesAsync();
            return ToDto(company);
        }

        public async Task DeleteAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);

            if (company is null)
                throw new NotFoundException($"Company with id '{id}' does not exist.");

            _companyRepository.Delete(company);
            await _companyRepository.SaveChangesAsync();
        }
    }
}