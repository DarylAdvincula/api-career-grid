using CG.DAL;
using CG.Models;
using Microsoft.EntityFrameworkCore;

namespace CG.Tests.Infrastructure;

public class CareerGridTestDbContext : AppDbContext
{
    public CareerGridTestDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicantProfile>().ToTable("ApplicantProfiles");
        modelBuilder.Entity<ApplicantProfile>().Property(entity => entity.Id).HasColumnName("ApplicantProfileID");
        modelBuilder.Entity<ApplicantProfile>().Property(entity => entity.UserAccountId).HasColumnName("UserID");

        modelBuilder.Entity<ApplicantSkill>().ToTable("ApplicantSkills");
        modelBuilder.Entity<ApplicantSkill>().Property(entity => entity.Id).HasColumnName("ApplicantSkillID");

        modelBuilder.Entity<Application>().ToTable("Applications");
        modelBuilder.Entity<Application>().Property(entity => entity.Id).HasColumnName("ApplicationID");
        modelBuilder.Entity<Application>().Property(entity => entity.ApplicationStatus).HasColumnName("Status");

        modelBuilder.Entity<ApplicationStatusLog>().ToTable("ApplicationStatusLogs");
        modelBuilder.Entity<ApplicationStatusLog>().Property(entity => entity.Id).HasColumnName("StatusLogID");
        modelBuilder.Entity<ApplicationStatusLog>().Property(entity => entity.OldApplicationStatus).HasColumnName("OldStatus");
        modelBuilder.Entity<ApplicationStatusLog>().Property(entity => entity.NewApplicationStatus).HasColumnName("NewStatus");

        modelBuilder.Entity<Company>().ToTable("Companies");
        modelBuilder.Entity<Company>().Property(entity => entity.Id).HasColumnName("CompanyID");
        modelBuilder.Entity<Company>().Property(entity => entity.Name).HasColumnName("CompanyName");
        modelBuilder.Entity<Company>().Property(entity => entity.Description).HasColumnName("CompanyDescription");
        modelBuilder.Entity<Company>().Property(entity => entity.Code).HasColumnName("CompanyCode");

        modelBuilder.Entity<EmployerProfile>().ToTable("EmployerProfiles");
        modelBuilder.Entity<EmployerProfile>().Property(entity => entity.Id).HasColumnName("EmployerProfileID");
        modelBuilder.Entity<EmployerProfile>().Property(entity => entity.UserAccountId).HasColumnName("UserID");
        modelBuilder.Entity<EmployerProfile>().Property(entity => entity.ApprovedByMemberId).HasColumnName("ApprovedByUserID");

        modelBuilder.Entity<JobClassification>().ToTable("JobClassifications");
        modelBuilder.Entity<JobClassification>().Property(entity => entity.Id).HasColumnName("ClassificationID");

        modelBuilder.Entity<JobPosting>().ToTable("JobPostings");
        modelBuilder.Entity<JobPosting>().Property(entity => entity.Id).HasColumnName("JobPostingID");
        modelBuilder.Entity<JobPosting>().Property(entity => entity.CreatedByEmployerId).HasColumnName("CreatedByUserID");
        modelBuilder.Entity<JobPosting>().Property(entity => entity.JobClassificationId).HasColumnName("ClassificationID");
        modelBuilder.Entity<JobPosting>().Property(entity => entity.JobSubClassificationId).HasColumnName("SubClassificationID");
        modelBuilder.Entity<JobPosting>().Property(entity => entity.ApprovedByAdminId).HasColumnName("ApprovedByUserID");

        modelBuilder.Entity<JobPostingSkill>().ToTable("JobPostingSkills");
        modelBuilder.Entity<JobPostingSkill>().Property(entity => entity.Id).HasColumnName("JobPostingSkillID");

        modelBuilder.Entity<JobSubClassification>().ToTable("JobSubClassifications");
        modelBuilder.Entity<JobSubClassification>().Property(entity => entity.Id).HasColumnName("SubClassificationID");
        modelBuilder.Entity<JobSubClassification>().Property(entity => entity.JobClassificationId).HasColumnName("ClassificationID");

        modelBuilder.Entity<Resume>().ToTable("Resumes");
        modelBuilder.Entity<Resume>().Property(entity => entity.Id).HasColumnName("ResumeID");

        modelBuilder.Entity<Skill>().ToTable("Skills");
        modelBuilder.Entity<Skill>().Property(entity => entity.Id).HasColumnName("SkillID");
        modelBuilder.Entity<Skill>().Property(entity => entity.JobClassificationId).HasColumnName("ClassificationID");
        modelBuilder.Entity<Skill>().Property(entity => entity.JobSubClassificationId).HasColumnName("SubClassificationID");

        modelBuilder.Entity<UserAccount>().ToTable("UserAccounts");
        modelBuilder.Entity<UserAccount>().Property(entity => entity.Id).HasColumnName("UserID");

        modelBuilder.Entity<WorkExperience>().ToTable("WorkExperiences");
        modelBuilder.Entity<WorkExperience>().Property(entity => entity.Id).HasColumnName("WorkExperienceID");
    }
}