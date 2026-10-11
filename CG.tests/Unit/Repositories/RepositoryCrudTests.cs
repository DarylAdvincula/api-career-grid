using CG.Models;
using CG.Repositories.Implementations;
using CG.Tests.Infrastructure;
using FluentAssertions;
using Xunit;

namespace CG.Tests.Unit.Repositories;

public class RepositoryCrudTests
{
    [Fact]
    public async Task ApplicantProfileRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ApplicantProfileRepository(context);
        await VerifyCrudAsync(
            new ApplicantProfile { UserAccountId = 12, Headline = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, profile => profile.Headline = "Updated",
            profile => profile.Headline == "Updated", profile => profile.Id);
    }

    [Fact]
    public async Task ApplicantSkillRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ApplicantSkillRepository(context);
        await VerifyCrudAsync(
            new ApplicantSkill { ApplicantProfileId = 12, SkillId = 5, YearsOfExperience = 1 },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, skill => skill.YearsOfExperience = 4,
            skill => skill.YearsOfExperience == 4, skill => skill.Id);
    }

    [Fact]
    public async Task ApplicationRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ApplicationRepository(context);
        await VerifyCrudAsync(
            new Application { JobPostingId = 7, ApplicantProfileId = 12, CoverLetter = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, application => application.CoverLetter = "Updated",
            application => application.CoverLetter == "Updated", application => application.Id);
    }

    [Fact]
    public async Task ApplicationStatusLogRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ApplicationStatusLogRepository(context);
        await VerifyCrudAsync(
            new ApplicationStatusLog { ApplicationId = 7, ChangedByUserId = 3, Notes = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, log => log.Notes = "Updated",
            log => log.Notes == "Updated", log => log.Id);
    }

    [Fact]
    public async Task CompanyRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new CompanyRepository(context);
        await VerifyCrudAsync(
            new Company { Name = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, company => company.Name = "Updated",
            company => company.Name == "Updated", company => company.Id);
    }

    [Fact]
    public async Task EmployerProfileRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new EmployerProfileRepository(context);
        await VerifyCrudAsync(
            new EmployerProfile { UserAccountId = 4, CompanyId = 8 },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, profile => profile.CompanyRole = CompanyRole.Admin,
            profile => profile.CompanyRole == CompanyRole.Admin, profile => profile.Id);
    }

    [Fact]
    public async Task JobClassificationRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new JobClassificationRepository(context);
        await VerifyCrudAsync(
            new JobClassification { Name = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, classification => classification.Name = "Updated",
            classification => classification.Name == "Updated", classification => classification.Id);
    }

    [Fact]
    public async Task JobPostingRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new JobPostingRepository(context);
        await VerifyCrudAsync(
            new JobPosting { CompanyId = 9, CreatedByEmployerId = 4, Title = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, posting => posting.Title = "Updated",
            posting => posting.Title == "Updated", posting => posting.Id);
    }

    [Fact]
    public async Task JobPostingSkillRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new JobPostingSkillRepository(context);
        await VerifyCrudAsync(
            new JobPostingSkill { JobPostingId = 9, SkillId = 5, IsRequired = true },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, link => link.IsRequired = false,
            link => !link.IsRequired, link => link.Id);
    }

    [Fact]
    public async Task JobSubClassificationRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new JobSubClassificationRepository(context);
        await VerifyCrudAsync(
            new JobSubClassification { JobClassificationId = 5, Name = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, classification => classification.Name = "Updated",
            classification => classification.Name == "Updated", classification => classification.Id);
    }

    [Fact]
    public async Task ResumeRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new ResumeRepository(context);
        await VerifyCrudAsync(
            new Resume { ApplicantProfileId = 12, OriginalFileName = "old.pdf", FilePath = "old.pdf" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, resume => resume.OriginalFileName = "updated.pdf",
            resume => resume.OriginalFileName == "updated.pdf", resume => resume.Id);
    }

    [Fact]
    public async Task SkillRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new SkillRepository(context);
        await VerifyCrudAsync(
            new Skill { Name = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, skill => skill.Name = "Updated",
            skill => skill.Name == "Updated", skill => skill.Id);
    }

    [Fact]
    public async Task UserAccountRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new UserAccountRepository(context);
        await VerifyCrudAsync(
            new UserAccount { Email = "person@example.com", FirstName = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, account => account.FirstName = "Updated",
            account => account.FirstName == "Updated", account => account.Id);
    }

    [Fact]
    public async Task WorkExperienceRepository_SavesUpdatesAndDeletes()
    {
        await using var context = TestDbContextFactory.Create();
        var repository = new WorkExperienceRepository(context);
        await VerifyCrudAsync(
            new WorkExperience { ApplicantProfileId = 12, JobTitle = "Old" },
            repository.AddAsync, repository.GetByIdAsync, repository.Update, repository.Delete,
            repository.SaveChangesAsync, experience => experience.JobTitle = "Updated",
            experience => experience.JobTitle == "Updated", experience => experience.Id);
    }

    private static async Task VerifyCrudAsync<TEntity>(
        TEntity entity,
        Func<TEntity, Task> add,
        Func<int, Task<TEntity?>> getById,
        Action<TEntity> update,
        Action<TEntity> delete,
        Func<Task> saveChanges,
        Action<TEntity> applyUpdate,
        Func<TEntity, bool> isUpdated,
        Func<TEntity, int> getId)
        where TEntity : class
    {
        await add(entity);
        await saveChanges();

        var id = getId(entity);
        id.Should().BePositive();
        var saved = await getById(id);
        saved.Should().NotBeNull();

        applyUpdate(saved!);
        update(saved!);
        await saveChanges();

        var updated = await getById(id);
        updated.Should().NotBeNull();
        isUpdated(updated!).Should().BeTrue();

        delete(updated!);
        await saveChanges();
        (await getById(id)).Should().BeNull();
    }
}
