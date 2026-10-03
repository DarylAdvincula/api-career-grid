using CG.Models;
using Microsoft.EntityFrameworkCore;

namespace CG.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) {}

        public DbSet<ApplicantProfile> ApplicantProfiles { get; set; }
        public DbSet<ApplicantSkill> ApplicantSkills { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationStatusLog> ApplicationStatusLogs { get; set; }
        public DbSet<Company> Companys { get; set; }
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
        public DbSet<JobClassification> JobClassifications { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<JobPostingSkill> JobPostingSkills { get; set; }
        public DbSet<JobSubClassification> JobSubClassifications { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JobClassification>()
                .ToTable("JobClassifications");

            modelBuilder.Entity<JobSubClassification>()
                .ToTable("JobSubClassification");

            modelBuilder.Entity<Skill>()
                .ToTable("Skills");

            modelBuilder.Entity<UserAccount>()
                .ToTable("UserAccounts");

            modelBuilder.Entity<Company>()
                .ToTable("Companys");

            modelBuilder.Entity<EmployerProfile>()
                .ToTable("EmployerProfiles");

            modelBuilder.Entity<ApplicantProfile>()
                .ToTable("ApplicantProfiles");

            modelBuilder.Entity<WorkExperience>()
                .ToTable("WorkExperiences");

            modelBuilder.Entity<Resume>()
                .ToTable("Resumes");

            modelBuilder.Entity<ApplicantSkill>()
                .ToTable("ApplicantSkills");

            modelBuilder.Entity<JobPosting>()
                .ToTable("JobPostings");

            modelBuilder.Entity<JobPostingSkill>()
                .ToTable("JobPostingSkills");

            modelBuilder.Entity<Application>()
                .ToTable("Applications");

            modelBuilder.Entity<ApplicationStatusLog>()
                .ToTable("ApplicationStatusLogs");


            modelBuilder.Entity<JobSubClassification>()
                .HasIndex(e => new
                {
                    e.JobClassificationId,
                    e.Name
                })
                .IsUnique();
            
            modelBuilder.Entity<Skill>()
                .HasOne(e => e.JobClassification)
                .WithMany()
                .HasForeignKey(e => e.JobClassificationId)
                .OnDelete(DeleteBehavior.SetNull);
            
            modelBuilder.Entity<Skill>()
                .HasOne(e => e.JobSubClassification)
                .WithMany()
                .HasForeignKey(e => e.JobSubClassificationId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Skill>()
                .HasIndex(e => e.Name);
            modelBuilder.Entity<Skill>()
                .HasIndex(e => e.JobClassificationId);
            modelBuilder.Entity<Skill>()
                .HasIndex(e => e.JobSubClassificationId);

            modelBuilder.Entity<UserAccount>()
                .Property(e => e.Role)
                .HasConversion<string>();
            
            modelBuilder.Entity<UserAccount>()
                .ToTable("UserAccount", e =>
                {
                    e.HasCheckConstraint(
                        "CK_UserAccounts_Role",
                        "Role IN ('Applicant', 'Employer', 'Admin')"
                    );
                    e.HasCheckConstraint(
                        "CK_UserAccounts_EmailFormat",
                        "Email LIKE '%_@__%.__%'"
                    );
                });

            modelBuilder.Entity<UserAccount>()
                .HasIndex(e => e.Email)
                .IsUnique();
            
            modelBuilder.Entity<Company>()
                .Property(e => e.VerificationStatus)
                .HasConversion<string>();
            
            modelBuilder.Entity<Company>()
                .ToTable("Companies", e =>
                {
                    e.HasCheckConstraint(
                        "CK_Companies_Status",
                        "VerificationStatus IN ('Pending', 'Approved', 'Rejected')"
                    );
                });
            
            modelBuilder.Entity<Company>()
                .HasOne(e => e.ApprovedByAdmin)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByAdminId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployerProfile>()
                .Property(e => e.MembershipStatus)
                .HasConversion<string>();

            modelBuilder.Entity<EmployerProfile>()
                .ToTable("EmployerProfiles", e =>
                {
                    e.HasCheckConstraint(
                        "CK_EmployerProfiles_Status",
                        "MembershipStatus IN ('Pending', 'Approved', 'Rejected')"
                    );
                });
            
            modelBuilder.Entity<EmployerProfile>()
                .HasOne(e => e.ApprovedByMember)
                .WithMany()
                .HasForeignKey(e => e.ApproverByMemberId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployerProfile>()
                .HasIndex(e => e.CompanyId);
            
            modelBuilder.Entity<WorkExperience>()
                .ToTable("WorkExperience", e =>
                {
                    e.HasCheckConstraint(
                        "CK_WorkExperiences_CurrentRole",
                        "(IsCurrentRole = 1 AND EndDate IS NULL) OR (IsCurrentRole = 0)"
                    );
                });

            modelBuilder.Entity<WorkExperience>()
                .HasIndex(e => e.ApplicantProfileId);

            modelBuilder.Entity<Resume>()
                .HasIndex(e => e.ApplicantProfileId);

            modelBuilder.Entity<Resume>()
                .HasIndex(e => e.ApplicantProfileId)
                .IsUnique()
                .HasFilter("[IsPrimary] = 1")
                .HasDatabaseName("UX_Resumes_PrimaryPerApplicant");

            modelBuilder.Entity<ApplicantSkill>()
                .Property(e => e.ProficiencyLevel)
                .HasConversion<string>();

            modelBuilder.Entity<ApplicantSkill>()
                .ToTable("ApplicantSkill", e =>
                {
                    e.HasCheckConstraint(
                        "CK_ApplicantSkills_Proficiency",
                        "ProficiencyLevel IS NULL OR ProficiencyLevel IN ('Beginner', 'Intermediate', 'Advanced', 'Expert')"
                    );
                });
            
            modelBuilder.Entity<ApplicantSkill>()
                .HasIndex(e => new
                {
                    e.ApplicantProfileId,
                    e.SkillId
                })
                .IsUnique();
            
            modelBuilder.Entity<ApplicantSkill>()
                .HasIndex(e => e.SkillId);
            
            modelBuilder.Entity<JobPosting>()
                .Property(e => e.ApprovalStatus)
                .HasConversion<string>();
            
            modelBuilder.Entity<JobPosting>()
                .Property(e => e.JobType)
                .HasConversion<string>();
            
            modelBuilder.Entity<JobPosting>()
                .Property(e => e.WorkEnvironment)
                .HasConversion<string>();
            
            modelBuilder.Entity<JobPosting>()
                .ToTable("JobPosting", e =>
                {
                    e.HasCheckConstraint(
                        "CK_JobPostings_ApprovalStatus",
                        "ApprovalStatus IN ('Draft', 'Pending', 'Published', 'Rejected')"
                    );
                    e.HasCheckConstraint(
                        "CK_JobPostings_JobType",
                        "JobType IS NULL OR JobType IN ('Full_Time', 'Part_Time', 'Contractual_Temporary')"
                    );
                    e.HasCheckConstraint(
                        "CK_JobPostings_WorkEnvironment",
                        "WorkEnvironment IS NULL OR WorkEnvironment IN ('On_Site', 'Hybrid', 'Remote')"
                    );
                    e.HasCheckConstraint(
                        "CK_JobPostings_SalaryRange",
                        "MaxSalary IS NULL OR MinSalary IS NULL OR MaxSalary >= MinSalary"
                    );
                });
            
            modelBuilder.Entity<JobPosting>()
                .HasOne(e => e.CreatedByEmployer)
                .WithMany()
                .HasForeignKey(e => e.CreatedByEmployerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<JobPosting>()
                .HasOne(e => e.ApprovedByAdmin)
                .WithMany()
                .HasForeignKey(e => e.ApprovedByAdminId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<JobPosting>()
                .HasIndex(e => e.CompanyId);
            modelBuilder.Entity<JobPosting>()
                .HasIndex(e => e.JobClassificationId);
            modelBuilder.Entity<JobPosting>()
                .HasIndex(e => e.JobSubClassificationId);

            modelBuilder.Entity<JobPostingSkill>()
                .HasIndex(e => new
                {
                    e.JobPostingId,
                    e.SkillId
                })
                .IsUnique();
            
            modelBuilder.Entity<JobPostingSkill>()
                .HasIndex(e => e.SkillId);
            
            modelBuilder.Entity<Application>()
                .Property(e => e.ApplicationStatus)
                .HasConversion<string>();

            modelBuilder.Entity<Application>()
                .ToTable("Applications", e =>
                {
                    e.HasCheckConstraint(
                        "CK_Applications_Status",
                        "ApplicationStatus IN ('Draft', 'Applied', 'Screening', 'Interviewing', 'Hired', 'Rejected')"
                    );
                });
            
            modelBuilder.Entity<Application>()
                .HasIndex(e => new
                {
                    e.JobPostingId,
                    e.ApplicantProfileId
                })
                .IsUnique();

            modelBuilder.Entity<Application>()
                .HasIndex(e => e.JobPostingId);
            modelBuilder.Entity<Application>()
                .HasIndex(e => e.ApplicantProfileId);
            
            modelBuilder.Entity<ApplicationStatusLog>()
                .HasOne(e => e.ChangedByUser)
                .WithMany()
                .HasForeignKey(e => e.ChangedByUserId)
                .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<ApplicationStatusLog>()
                .HasIndex(e => e.ApplicationId);
        }
    }
}