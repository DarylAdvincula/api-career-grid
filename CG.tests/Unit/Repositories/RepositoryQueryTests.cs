using CG.Models;
using CG.Repositories.Implementations;
using CG.Tests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace CG.Tests.Unit.Repositories;

public class RepositoryQueryTests
{
    // GetAllAsync Tests
    [Fact]
    public async Task ApplicantProfileRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.ApplicantProfiles.Add(new ApplicantProfile { Id = 1, UserAccountId = 2 });
        await context.SaveChangesAsync();

        var result = await new ApplicantProfileRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task ApplicantSkillRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.ApplicantSkills.Add(new ApplicantSkill { Id = 1, ApplicantProfileId = 1, SkillId = 3 });
        await context.SaveChangesAsync();

        var result = await new ApplicantSkillRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task ApplicationRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.Applications.Add(new Application { Id = 1, JobPostingId = 4, ApplicantProfileId = 1 });
        await context.SaveChangesAsync();

        var result = await new ApplicationRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task ApplicationStatusLogRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.ApplicationStatusLogs.Add(new ApplicationStatusLog { Id = 1, ApplicationId = 1, ChangedByUserId = 2 });
        await context.SaveChangesAsync();

        var result = await new ApplicationStatusLogRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task CompanyRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.Companies.Add(new Company { Id = 1, Name = "Example" });
        await context.SaveChangesAsync();

        var result = await new CompanyRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task EmployerProfileRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.EmployerProfiles.Add(new EmployerProfile { Id = 1, UserAccountId = 2, CompanyId = 1 });
        await context.SaveChangesAsync();

        var result = await new EmployerProfileRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task JobClassificationRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobClassifications.Add(new JobClassification { Id = 1, Name = "Engineering" });
        await context.SaveChangesAsync();

        var result = await new JobClassificationRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task JobPostingRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobPostings.Add(new JobPosting { Id = 1, CompanyId = 1, CreatedByEmployerId = 2, Title = "Developer" });
        await context.SaveChangesAsync();

        var result = await new JobPostingRepository(context).GetAllAsync(null);
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task JobPostingSkillRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobPostingSkills.Add(new JobPostingSkill { Id = 1, JobPostingId = 1, SkillId = 3 });
        await context.SaveChangesAsync();

        var result = await new JobPostingSkillRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task JobSubClassificationRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobSubClassifications.Add(new JobSubClassification { Id = 1, JobClassificationId = 1, Name = "Backend" });
        await context.SaveChangesAsync();

        var result = await new JobSubClassificationRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task ResumeRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.Resumes.Add(new Resume { Id = 1, ApplicantProfileId = 1, OriginalFileName = "resume.pdf" });
        await context.SaveChangesAsync();

        var result = await new ResumeRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task SkillRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.Skills.Add(new Skill { Id = 3, Name = "C#" });
        await context.SaveChangesAsync();

        var result = await new SkillRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task UserAccountRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.UserAccounts.Add(new UserAccount { Id = 2, Email = "person@example.com" });
        await context.SaveChangesAsync();

        var result = await new UserAccountRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    [Fact]
    public async Task WorkExperienceRepository_GetAllAsync_ReturnsSeededRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.WorkExperiences.Add(new WorkExperience { Id = 1, ApplicantProfileId = 1, JobTitle = "Developer" });
        await context.SaveChangesAsync();

        var result = await new WorkExperienceRepository(context).GetAllAsync();
        result.Should().ContainSingle();
    }

    // Lookup Methods

    [Fact]
    public async Task ApplicantProfileRepository_GetByUserAccountIdAsync_FindsCorrectRow()
    {
        await using var context = TestDbContextFactory.Create();
        context.UserAccounts.Add(new UserAccount { Id = 2, Email = "person@example.com" });
        context.ApplicantProfiles.Add(new ApplicantProfile { Id = 3, UserAccountId = 2 });
        await context.SaveChangesAsync();

        var result = await new ApplicantProfileRepository(context).GetByUserAccountIdAsync(2);
        result.Should().NotBeNull();
        result!.Id.Should().Be(3);
    }

    [Fact]
    public async Task EmployerProfileRepository_GetByUserAccountIdAsync_FindsCorrectRow()
    {
        await using var context = TestDbContextFactory.Create();
        context.UserAccounts.Add(new UserAccount { Id = 2, Email = "person@example.com" });
        context.Companies.Add(new Company { Id = 4, Name = "Example" });
        context.EmployerProfiles.Add(new EmployerProfile { Id = 5, UserAccountId = 2, CompanyId = 4 });
        await context.SaveChangesAsync();

        var result = await new EmployerProfileRepository(context).GetByUserAccountIdAsync(2);
        result.Should().NotBeNull();
        result!.Id.Should().Be(5);
    }

    [Fact]
    public async Task JobClassificationRepository_GetByNameAsync_FindsClassification()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobClassifications.Add(new JobClassification { Id = 6, Name = "Engineering" });
        await context.SaveChangesAsync();

        var result = await new JobClassificationRepository(context).GetByNameAsync("engineering");
        result.Should().NotBeNull();
        result!.Id.Should().Be(6);
    }

    [Fact]
    public async Task SkillRepository_GetByNameAsync_FindsSkill()
    {
        await using var context = TestDbContextFactory.Create();
        context.Skills.Add(new Skill { Id = 7, Name = "C#" });
        await context.SaveChangesAsync();

        var result = await new SkillRepository(context).GetByNameAsync("C#");
        result.Should().NotBeNull();
        result!.Id.Should().Be(7);
    }

    [Fact]
    public async Task UserAccountRepository_GetByEmailAsync_FindsAccount()
    {
        await using var context = TestDbContextFactory.Create();
        context.UserAccounts.Add(new UserAccount { Id = 2, Email = "person@example.com" });
        await context.SaveChangesAsync();

        var result = await new UserAccountRepository(context).GetByEmailAsync("person@example.com");
        result.Should().NotBeNull();
        result!.Id.Should().Be(2);
    }

    [Fact]
    public async Task UserAccountRepository_EmailExistsAsync_ValidatesPresence()
    {
        await using var context = TestDbContextFactory.Create();
        context.UserAccounts.Add(new UserAccount { Id = 2, Email = "person@example.com" });
        await context.SaveChangesAsync();

        var repo = new UserAccountRepository(context);
        (await repo.EmailExistsAsync("person@example.com")).Should().BeTrue();
        (await repo.EmailExistsAsync("missing@example.com")).Should().BeFalse();
    }

    [Fact]
    public async Task ApplicationStatusLogRepository_GetByApplicationIdAsync_FindsLog()
    {
        await using var context = TestDbContextFactory.Create();
        context.Applications.Add(new Application { Id = 8, JobPostingId = 9, ApplicantProfileId = 3 });
        context.ApplicationStatusLogs.Add(new ApplicationStatusLog { Id = 10, ApplicationId = 8, ChangedByUserId = 2 });
        await context.SaveChangesAsync();

        var result = await new ApplicationStatusLogRepository(context).GetByApplicationIdAsync(8);
        result.Should().NotBeNull();
        result!.Id.Should().Be(10);
    }

    // Parent, Relationship & Search Queries

    [Fact]
    public async Task ApplicantSkillRepository_GetAllByApplicantProfileIdAsync_ReturnsFilteredRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.ApplicantSkills.AddRange(
            new ApplicantSkill { Id = 41, ApplicantProfileId = 1, SkillId = 51 },
            new ApplicantSkill { Id = 42, ApplicantProfileId = 2, SkillId = 52 });
        await context.SaveChangesAsync();

        var result = await new ApplicantSkillRepository(context).GetAllByApplicantProfileIdAsync(1);
        result.Should().ContainSingle().Which.Id.Should().Be(41);
    }

    [Fact]
    public async Task ApplicationRepository_GetAllByApplicantProfileIdAsync_ReturnsFilteredRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.Applications.AddRange(
            new Application { Id = 61, JobPostingId = 21, ApplicantProfileId = 1 },
            new Application { Id = 62, JobPostingId = 22, ApplicantProfileId = 1 });
        await context.SaveChangesAsync();

        var result = await new ApplicationRepository(context).GetAllByApplicantProfileIdAsync(1);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task WorkExperienceRepository_GetAllByApplicantProfileIdAsync_ReturnsFilteredRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.WorkExperiences.AddRange(
            new WorkExperience { Id = 71, ApplicantProfileId = 1, JobTitle = "Engineer" },
            new WorkExperience { Id = 72, ApplicantProfileId = 2, JobTitle = "Designer" });
        await context.SaveChangesAsync();

        var result = await new WorkExperienceRepository(context).GetAllByApplicantProfileIdAsync(1);
        result.Should().ContainSingle().Which.Id.Should().Be(71);
    }

    [Fact]
    public async Task ResumeRepository_GetAllByApplicantProfileIdAsync_ReturnsFilteredRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.Resumes.AddRange(
            new Resume { Id = 81, ApplicantProfileId = 1, IsPrimary = false },
            new Resume { Id = 82, ApplicantProfileId = 1, IsPrimary = true },
            new Resume { Id = 83, ApplicantProfileId = 2, IsPrimary = true });
        await context.SaveChangesAsync();

        var result = await new ResumeRepository(context).GetAllByApplicantProfileIdAsync(1);
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task ResumeRepository_GetPrimaryByApplicantProfileId_ReturnsPrimaryResume()
    {
        await using var context = TestDbContextFactory.Create();
        context.Resumes.AddRange(
            new Resume { Id = 81, ApplicantProfileId = 1, IsPrimary = false },
            new Resume { Id = 82, ApplicantProfileId = 1, IsPrimary = true });
        await context.SaveChangesAsync();

        var result = await new ResumeRepository(context).GetPrimaryByApplicantProfileId(1);
        result.Should().NotBeNull();
        result!.Id.Should().Be(82);
    }

    [Fact]
    public async Task JobPostingSkillRepository_GetAllByJobPostingIdAsync_ReturnsFilteredRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobPostingSkills.AddRange(
            new JobPostingSkill { Id = 91, JobPostingId = 21, SkillId = 51 },
            new JobPostingSkill { Id = 92, JobPostingId = 22, SkillId = 52 });
        await context.SaveChangesAsync();

        var result = await new JobPostingSkillRepository(context).GetAllByJobPostingIdAsync(21);
        result.Should().ContainSingle().Which.Id.Should().Be(91);
    }

    [Fact]
    public async Task JobPostingRepository_GetAllByCompanyIdAsync_ReturnsFilteredRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobPostings.AddRange(
            new JobPosting { Id = 21, CompanyId = 31, CreatedByEmployerId = 11, Title = "Senior C# Engineer" },
            new JobPosting { Id = 22, CompanyId = 32, CreatedByEmployerId = 12, Title = "Designer" });
        await context.SaveChangesAsync();

        var result = await new JobPostingRepository(context).GetAllByCompanyIdAsync(31);
        result.Should().ContainSingle().Which.Id.Should().Be(21);
    }

    [Fact]
    public async Task JobPostingRepository_GetAllAsync_WithSearchTerm_ReturnsMatchingRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.JobPostings.AddRange(
            new JobPosting { Id = 21, CompanyId = 31, CreatedByEmployerId = 11, Title = "Senior C# Engineer" },
            new JobPosting { Id = 22, CompanyId = 32, CreatedByEmployerId = 12, Title = "Designer" });
        await context.SaveChangesAsync();

        var repo = new JobPostingRepository(context);
        
        // Search match
        (await repo.GetAllAsync("C#")).Should().ContainSingle().Which.Id.Should().Be(21);
        
        // Blank search returns all
        (await repo.GetAllAsync(" ")).Should().HaveCount(2);
    }

    [Fact]
    public async Task ApplicationStatusLogRepository_GetAllByApplicantProfileIdAsync_ReturnsFilteredRows()
    {
        await using var context = TestDbContextFactory.Create();
        context.Applications.Add(new Application { Id = 61, JobPostingId = 21, ApplicantProfileId = 1 });
        context.ApplicationStatusLogs.Add(new ApplicationStatusLog { Id = 111, ApplicationId = 61, ChangedByUserId = 11 });
        await context.SaveChangesAsync();

        var result = await new ApplicationStatusLogRepository(context).GetAllByApplicantProfileIdAsync(1);
        result.Should().ContainSingle().Which.Id.Should().Be(111);
    }
}