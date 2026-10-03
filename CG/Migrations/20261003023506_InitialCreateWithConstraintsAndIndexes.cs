using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateWithConstraintsAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobClassifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobClassifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsVerified = table.Column<bool>(type: "bit", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerificationExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccount", x => x.Id);
                    table.CheckConstraint("CK_UserAccounts_EmailFormat", "Email LIKE '%_@__%.__%'");
                    table.CheckConstraint("CK_UserAccounts_Role", "Role IN ('Applicant', 'Employer', 'Admin')");
                });

            migrationBuilder.CreateTable(
                name: "JobSubClassification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobClassificationId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSubClassification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSubClassification_JobClassifications_JobClassificationId",
                        column: x => x.JobClassificationId,
                        principalTable: "JobClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    Headline = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    HomeLocation = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicantProfiles_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApprovedByAdminId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(2083)", maxLength: 2083, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    VerificationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                    table.CheckConstraint("CK_Companies_Status", "VerificationStatus IN ('Pending', 'Approved', 'Rejected')");
                    table.ForeignKey(
                        name: "FK_Companies_UserAccount_ApprovedByAdminId",
                        column: x => x.ApprovedByAdminId,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobClassificationId = table.Column<int>(type: "int", nullable: true),
                    JobSubClassificationId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_JobClassifications_JobClassificationId",
                        column: x => x.JobClassificationId,
                        principalTable: "JobClassifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Skills_JobSubClassification_JobSubClassificationId",
                        column: x => x.JobSubClassificationId,
                        principalTable: "JobSubClassification",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantProfileId = table.Column<int>(type: "int", nullable: false),
                    OriginalFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StoredFileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSizeInBytes = table.Column<int>(type: "int", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resumes_ApplicantProfiles_ApplicantProfileId",
                        column: x => x.ApplicantProfileId,
                        principalTable: "ApplicantProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperience",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantProfileId = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrentRole = table.Column<bool>(type: "bit", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperience", x => x.Id);
                    table.CheckConstraint("CK_WorkExperiences_CurrentRole", "(IsCurrentRole = 1 AND EndDate IS NULL) OR (IsCurrentRole = 0)");
                    table.ForeignKey(
                        name: "FK_WorkExperience_ApplicantProfiles_ApplicantProfileId",
                        column: x => x.ApplicantProfileId,
                        principalTable: "ApplicantProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmployerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserAccountId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ApproverByMemberId = table.Column<int>(type: "int", nullable: false),
                    CompanyRole = table.Column<int>(type: "int", nullable: false),
                    MembershipStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployerProfiles", x => x.Id);
                    table.CheckConstraint("CK_EmployerProfiles_Status", "MembershipStatus IN ('Pending', 'Approved', 'Rejected')");
                    table.ForeignKey(
                        name: "FK_EmployerProfiles_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployerProfiles_UserAccount_ApproverByMemberId",
                        column: x => x.ApproverByMemberId,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmployerProfiles_UserAccount_UserAccountId",
                        column: x => x.UserAccountId,
                        principalTable: "UserAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobPosting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreatedByEmployerId = table.Column<int>(type: "int", nullable: false),
                    JobClassificationId = table.Column<int>(type: "int", nullable: true),
                    JobSubClassificationId = table.Column<int>(type: "int", nullable: true),
                    ApprovedByAdminId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    RequirementsMarkdown = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    JobType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkEnvironment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinSalary = table.Column<decimal>(type: "money", nullable: true),
                    MaxSalary = table.Column<decimal>(type: "money", nullable: true),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPosting", x => x.Id);
                    table.CheckConstraint("CK_JobPostings_ApprovalStatus", "ApprovalStatus IN ('Draft', 'Pending', 'Published', 'Rejected')");
                    table.CheckConstraint("CK_JobPostings_JobType", "JobType IS NULL OR JobType IN ('Full_Time', 'Part_Time', 'Contractual_Temporary')");
                    table.CheckConstraint("CK_JobPostings_SalaryRange", "MaxSalary IS NULL OR MinSalary IS NULL OR MaxSalary >= MinSalary");
                    table.CheckConstraint("CK_JobPostings_WorkEnvironment", "WorkEnvironment IS NULL OR WorkEnvironment IN ('On_Site', 'Hybrid', 'Remote')");
                    table.ForeignKey(
                        name: "FK_JobPosting_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobPosting_JobClassifications_JobClassificationId",
                        column: x => x.JobClassificationId,
                        principalTable: "JobClassifications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobPosting_JobSubClassification_JobSubClassificationId",
                        column: x => x.JobSubClassificationId,
                        principalTable: "JobSubClassification",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobPosting_UserAccount_ApprovedByAdminId",
                        column: x => x.ApprovedByAdminId,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JobPosting_UserAccount_CreatedByEmployerId",
                        column: x => x.CreatedByEmployerId,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApplicantSkill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantProfileId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    ProficiencyLevel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantSkill", x => x.Id);
                    table.CheckConstraint("CK_ApplicantSkills_Proficiency", "ProficiencyLevel IS NULL OR ProficiencyLevel IN ('Beginner', 'Intermediate', 'Advanced', 'Expert')");
                    table.ForeignKey(
                        name: "FK_ApplicantSkill_ApplicantProfiles_ApplicantProfileId",
                        column: x => x.ApplicantProfileId,
                        principalTable: "ApplicantProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantSkill_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPostingId = table.Column<int>(type: "int", nullable: false),
                    ApplicantProfileId = table.Column<int>(type: "int", nullable: false),
                    ResumeId = table.Column<int>(type: "int", nullable: true),
                    IsSubmitted = table.Column<bool>(type: "bit", nullable: false),
                    DraftStep = table.Column<bool>(type: "bit", nullable: false),
                    CoverLetter = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ApplicationStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                    table.CheckConstraint("CK_Applications_Status", "ApplicationStatus IN ('Draft', 'Applied', 'Screening', 'Interviewing', 'Hired', 'Rejected')");
                    table.ForeignKey(
                        name: "FK_Applications_ApplicantProfiles_ApplicantProfileId",
                        column: x => x.ApplicantProfileId,
                        principalTable: "ApplicantProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_JobPosting_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPosting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applications_Resumes_ResumeId",
                        column: x => x.ResumeId,
                        principalTable: "Resumes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JobPostingSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobPostingId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    IsRequired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPostingSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPostingSkills_JobPosting_JobPostingId",
                        column: x => x.JobPostingId,
                        principalTable: "JobPosting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobPostingSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationStatusLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationId = table.Column<int>(type: "int", nullable: false),
                    ChangedByUserId = table.Column<int>(type: "int", nullable: false),
                    OldApplicationStatus = table.Column<int>(type: "int", nullable: true),
                    NewApplicationStatus = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatusLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApplicationStatusLogs_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationStatusLogs_UserAccount_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "UserAccount",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantProfiles_UserAccountId",
                table: "ApplicantProfiles",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantSkill_ApplicantProfileId_SkillId",
                table: "ApplicantSkill",
                columns: new[] { "ApplicantProfileId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantSkill_SkillId",
                table: "ApplicantSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicantProfileId",
                table: "Applications",
                column: "ApplicantProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobPostingId",
                table: "Applications",
                column: "JobPostingId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_JobPostingId_ApplicantProfileId",
                table: "Applications",
                columns: new[] { "JobPostingId", "ApplicantProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ResumeId",
                table: "Applications",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusLogs_ApplicationId",
                table: "ApplicationStatusLogs",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusLogs_ChangedByUserId",
                table: "ApplicationStatusLogs",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ApprovedByAdminId",
                table: "Companies",
                column: "ApprovedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_ApproverByMemberId",
                table: "EmployerProfiles",
                column: "ApproverByMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_CompanyId",
                table: "EmployerProfiles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_UserAccountId",
                table: "EmployerProfiles",
                column: "UserAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosting_ApprovedByAdminId",
                table: "JobPosting",
                column: "ApprovedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosting_CompanyId",
                table: "JobPosting",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosting_CreatedByEmployerId",
                table: "JobPosting",
                column: "CreatedByEmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosting_JobClassificationId",
                table: "JobPosting",
                column: "JobClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPosting_JobSubClassificationId",
                table: "JobPosting",
                column: "JobSubClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_JobPostingSkills_JobPostingId_SkillId",
                table: "JobPostingSkills",
                columns: new[] { "JobPostingId", "SkillId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JobPostingSkills_SkillId",
                table: "JobPostingSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSubClassification_JobClassificationId_Name",
                table: "JobSubClassification",
                columns: new[] { "JobClassificationId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Resumes_PrimaryPerApplicant",
                table: "Resumes",
                column: "ApplicantProfileId",
                unique: true,
                filter: "[IsPrimary] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_JobClassificationId",
                table: "Skills",
                column: "JobClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_JobSubClassificationId",
                table: "Skills",
                column: "JobSubClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name",
                table: "Skills",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccount_Email",
                table: "UserAccount",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperience_ApplicantProfileId",
                table: "WorkExperience",
                column: "ApplicantProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantSkill");

            migrationBuilder.DropTable(
                name: "ApplicationStatusLogs");

            migrationBuilder.DropTable(
                name: "EmployerProfiles");

            migrationBuilder.DropTable(
                name: "JobPostingSkills");

            migrationBuilder.DropTable(
                name: "WorkExperience");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "JobPosting");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "JobSubClassification");

            migrationBuilder.DropTable(
                name: "ApplicantProfiles");

            migrationBuilder.DropTable(
                name: "JobClassifications");

            migrationBuilder.DropTable(
                name: "UserAccount");
        }
    }
}
