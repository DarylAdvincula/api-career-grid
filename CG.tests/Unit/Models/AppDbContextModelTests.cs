using CG.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace CG.Tests.Unit.Models;

public class AppDbContextModelTests
{
    [Fact]
    public void Model_DeclaresUniqueEmailIndex()
    {
        using var context = TestDbContextFactory.Create();

        var emailIndex = context.Model.FindEntityType(typeof(CG.Models.UserAccount))!
            .GetIndexes()
            .Single(index => index.Properties.Any(property => property.Name == nameof(CG.Models.UserAccount.Email)));

        emailIndex.IsUnique.Should().BeTrue();
    }

    [Fact]
    public void Model_DeclaresUniquePrimaryResumeFilteredIndex()
    {
        using var context = TestDbContextFactory.Create();

        var primaryResumeIndex = context.Model.FindEntityType(typeof(CG.Models.Resume))!
            .GetIndexes()
            .Single(index => index.GetDatabaseName() == "UX_Resumes_PrimaryPerApplicant");

        primaryResumeIndex.IsUnique.Should().BeTrue();
        primaryResumeIndex.GetFilter().Should().Be("[IsPrimary] = 1");
    }

    [Fact]
    public void Model_DeclaresJobPostingSalaryCheckConstraint()
    {
        using var context = TestDbContextFactory.Create();

        var model = context.GetService<IDesignTimeModel>().Model;
        var constraint = model.FindEntityType(typeof(CG.Models.JobPosting))!
            .GetCheckConstraints()
            .Single(check => check.Name == "CK_JobPostings_SalaryRange");

        constraint.Sql.Should().Contain("MaxSalary >= MinSalary");
    }

    [Fact]
    public void Model_DeclaresExpectedDomainCheckConstraints()
    {
        using var context = TestDbContextFactory.Create();
        var model = context.GetService<IDesignTimeModel>().Model;
        var constraintNames = model.GetEntityTypes()
            .SelectMany(entity => entity.GetCheckConstraints())
            .Select(constraint => constraint.Name)
            .ToHashSet();

        constraintNames.Should().Contain(new[]
        {
            "CK_UserAccounts_Role",
            "CK_UserAccounts_EmailFormat",
            "CK_Companies_Status",
            "CK_EmployerProfiles_Status",
            "CK_WorkExperiences_CurrentRole",
            "CK_ApplicantSkills_Proficiency",
            "CK_JobPostings_ApprovalStatus",
            "CK_JobPostings_JobType",
            "CK_JobPostings_WorkEnvironment",
            "CK_JobPostings_SalaryRange",
            "CK_Applications_Status"
        });
    }

    [Fact]
    public void Model_DeclaresUniqueRelationshipIndexes()
    {
        using var context = TestDbContextFactory.Create();
        var model = context.GetService<IDesignTimeModel>().Model;

        IsUniqueIndex(model, typeof(CG.Models.JobSubClassification), "JobClassificationId", "Name").Should().BeTrue();
        IsUniqueIndex(model, typeof(CG.Models.ApplicantSkill), "ApplicantProfileId", "SkillId").Should().BeTrue();
        IsUniqueIndex(model, typeof(CG.Models.JobPostingSkill), "JobPostingId", "SkillId").Should().BeTrue();
        IsUniqueIndex(model, typeof(CG.Models.Application), "JobPostingId", "ApplicantProfileId").Should().BeTrue();
    }

    [Fact]
    public void Model_DeclaresExpectedLookupIndexes()
    {
        using var context = TestDbContextFactory.Create();
        var model = context.GetService<IDesignTimeModel>().Model;

        HasIndex(model, typeof(CG.Models.Skill), false, "Name").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.Skill), false, "JobClassificationId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.Skill), false, "JobSubClassificationId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.EmployerProfile), false, "CompanyId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.WorkExperience), false, "ApplicantProfileId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.ApplicantSkill), false, "SkillId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.JobPosting), false, "CompanyId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.JobPosting), false, "JobClassificationId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.JobPosting), false, "JobSubClassificationId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.JobPostingSkill), false, "SkillId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.Application), false, "JobPostingId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.Application), false, "ApplicantProfileId").Should().BeTrue();
        HasIndex(model, typeof(CG.Models.ApplicationStatusLog), false, "ApplicationId").Should().BeTrue();
    }

    [Fact]
    public void Model_DeclaresExpectedDeleteBehaviors()
    {
        using var context = TestDbContextFactory.Create();
        var model = context.GetService<IDesignTimeModel>().Model;

        GetDeleteBehavior(model, typeof(CG.Models.Skill), nameof(CG.Models.Skill.JobClassificationId))
            .Should().Be(DeleteBehavior.SetNull);
        GetDeleteBehavior(model, typeof(CG.Models.Skill), nameof(CG.Models.Skill.JobSubClassificationId))
            .Should().Be(DeleteBehavior.NoAction);
        GetDeleteBehavior(model, typeof(CG.Models.Company), nameof(CG.Models.Company.ApprovedByAdminId))
            .Should().Be(DeleteBehavior.NoAction);
        GetDeleteBehavior(model, typeof(CG.Models.EmployerProfile), nameof(CG.Models.EmployerProfile.ApprovedByMemberId))
            .Should().Be(DeleteBehavior.NoAction);
        GetDeleteBehavior(model, typeof(CG.Models.JobPosting), nameof(CG.Models.JobPosting.CreatedByEmployerId))
            .Should().Be(DeleteBehavior.NoAction);
        GetDeleteBehavior(model, typeof(CG.Models.JobPosting), nameof(CG.Models.JobPosting.ApprovedByAdminId))
            .Should().Be(DeleteBehavior.NoAction);
        GetDeleteBehavior(model, typeof(CG.Models.ApplicationStatusLog), nameof(CG.Models.ApplicationStatusLog.ChangedByUserId))
            .Should().Be(DeleteBehavior.NoAction);
    }

    [Fact]
    public void Model_MapsAllEntitiesToExpectedTables()
    {
        using var context = TestDbContextFactory.Create();
        var model = context.GetService<IDesignTimeModel>().Model;

        GetTableName(model, typeof(CG.Models.ApplicantProfile)).Should().Be("ApplicantProfiles");
        GetTableName(model, typeof(CG.Models.ApplicantSkill)).Should().Be("ApplicantSkill");
        GetTableName(model, typeof(CG.Models.Application)).Should().Be("Applications");
        GetTableName(model, typeof(CG.Models.ApplicationStatusLog)).Should().Be("ApplicationStatusLogs");
        GetTableName(model, typeof(CG.Models.Company)).Should().Be("Companies");
        GetTableName(model, typeof(CG.Models.EmployerProfile)).Should().Be("EmployerProfiles");
        GetTableName(model, typeof(CG.Models.JobClassification)).Should().Be("JobClassifications");
        GetTableName(model, typeof(CG.Models.JobPosting)).Should().Be("JobPosting");
        GetTableName(model, typeof(CG.Models.JobPostingSkill)).Should().Be("JobPostingSkills");
        GetTableName(model, typeof(CG.Models.JobSubClassification)).Should().Be("JobSubClassification");
        GetTableName(model, typeof(CG.Models.Resume)).Should().Be("Resumes");
        GetTableName(model, typeof(CG.Models.Skill)).Should().Be("Skills");
        GetTableName(model, typeof(CG.Models.UserAccount)).Should().Be("UserAccount");
        GetTableName(model, typeof(CG.Models.WorkExperience)).Should().Be("WorkExperience");
    }

    [Fact]
    public void Model_UsesExpectedStringLengthLimits()
    {
        using var context = TestDbContextFactory.Create();
        var model = context.GetService<IDesignTimeModel>().Model;

        GetMaxLength(model, typeof(CG.Models.UserAccount), nameof(CG.Models.UserAccount.Email)).Should().Be(256);
        GetMaxLength(model, typeof(CG.Models.JobPosting), nameof(CG.Models.JobPosting.Title)).Should().Be(150);
        GetMaxLength(model, typeof(CG.Models.JobPosting), nameof(CG.Models.JobPosting.RequirementsMarkdown)).Should().Be(5000);
        GetMaxLength(model, typeof(CG.Models.WorkExperience), nameof(CG.Models.WorkExperience.Description)).Should().Be(1500);
        GetMaxLength(model, typeof(CG.Models.Resume), nameof(CG.Models.Resume.FilePath)).Should().Be(500);
    }

    private static bool IsUniqueIndex(IModel model, Type entityType, params string[] propertyNames)
    {
        return model.FindEntityType(entityType)!
            .GetIndexes()
            .Any(index => index.IsUnique && index.Properties.Select(property => property.Name).SequenceEqual(propertyNames));
    }

    private static bool HasIndex(IModel model, Type entityType, bool isUnique, params string[] propertyNames)
    {
        return model.FindEntityType(entityType)!
            .GetIndexes()
            .Any(index => index.IsUnique == isUnique && index.Properties.Select(property => property.Name).SequenceEqual(propertyNames));
    }

    private static DeleteBehavior GetDeleteBehavior(IModel model, Type entityType, string foreignKeyProperty)
    {
        return model.FindEntityType(entityType)!
            .GetForeignKeys()
            .Single(foreignKey => foreignKey.Properties.Any(property => property.Name == foreignKeyProperty))
            .DeleteBehavior;
    }

    private static string? GetTableName(IModel model, Type entityType)
    {
        return model.FindEntityType(entityType)!.GetTableName();
    }

    private static int? GetMaxLength(IModel model, Type entityType, string propertyName)
    {
        return model.FindEntityType(entityType)!.FindProperty(propertyName)!.GetMaxLength();
    }
}