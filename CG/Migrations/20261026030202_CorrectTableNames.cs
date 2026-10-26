using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class CorrectTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantProfiles_UserAccount_UserAccountId",
                table: "ApplicantProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkill_ApplicantProfiles_ApplicantProfileId",
                table: "ApplicantSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkill_Skills_SkillId",
                table: "ApplicantSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobPosting_JobPostingId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStatusLogs_UserAccount_ChangedByUserId",
                table: "ApplicationStatusLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_UserAccount_ApprovedByAdminId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccount_ApprovedByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccount_UserAccountId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosting_Companies_CompanyId",
                table: "JobPosting");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosting_JobClassifications_JobClassificationId",
                table: "JobPosting");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosting_JobSubClassification_JobSubClassificationId",
                table: "JobPosting");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosting_UserAccount_ApprovedByAdminId",
                table: "JobPosting");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPosting_UserAccount_CreatedByEmployerId",
                table: "JobPosting");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostingSkills_JobPosting_JobPostingId",
                table: "JobPostingSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSubClassification_JobClassifications_JobClassificationId",
                table: "JobSubClassification");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_JobSubClassification_JobSubClassificationId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_UserAccount_UserAccountId",
                table: "UserSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperience_ApplicantProfiles_ApplicantProfileId",
                table: "WorkExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkExperience",
                table: "WorkExperience");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSubClassification",
                table: "JobSubClassification");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPosting",
                table: "JobPosting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicantSkill",
                table: "ApplicantSkill");

            migrationBuilder.RenameTable(
                name: "WorkExperience",
                newName: "WorkExperiences");

            migrationBuilder.RenameTable(
                name: "UserAccount",
                newName: "UserAccounst");

            migrationBuilder.RenameTable(
                name: "JobSubClassification",
                newName: "JobSubClassifications");

            migrationBuilder.RenameTable(
                name: "JobPosting",
                newName: "JobPostings");

            migrationBuilder.RenameTable(
                name: "ApplicantSkill",
                newName: "ApplicantSkills");

            migrationBuilder.RenameIndex(
                name: "IX_WorkExperience_ApplicantProfileId",
                table: "WorkExperiences",
                newName: "IX_WorkExperiences_ApplicantProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccount_Email",
                table: "UserAccounst",
                newName: "IX_UserAccounst_Email");

            migrationBuilder.RenameIndex(
                name: "IX_JobSubClassification_JobClassificationId_Name",
                table: "JobSubClassifications",
                newName: "IX_JobSubClassifications_JobClassificationId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_JobPosting_JobSubClassificationId",
                table: "JobPostings",
                newName: "IX_JobPostings_JobSubClassificationId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPosting_JobClassificationId",
                table: "JobPostings",
                newName: "IX_JobPostings_JobClassificationId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPosting_CreatedByEmployerId",
                table: "JobPostings",
                newName: "IX_JobPostings_CreatedByEmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPosting_CompanyId",
                table: "JobPostings",
                newName: "IX_JobPostings_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPosting_ApprovedByAdminId",
                table: "JobPostings",
                newName: "IX_JobPostings_ApprovedByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantSkill_SkillId",
                table: "ApplicantSkills",
                newName: "IX_ApplicantSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantSkill_ApplicantProfileId_SkillId",
                table: "ApplicantSkills",
                newName: "IX_ApplicantSkills_ApplicantProfileId_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkExperiences",
                table: "WorkExperiences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounst",
                table: "UserAccounst",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSubClassifications",
                table: "JobSubClassifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicantSkills",
                table: "ApplicantSkills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantProfiles_UserAccounst_UserAccountId",
                table: "ApplicantProfiles",
                column: "UserAccountId",
                principalTable: "UserAccounst",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkills_ApplicantProfiles_ApplicantProfileId",
                table: "ApplicantSkills",
                column: "ApplicantProfileId",
                principalTable: "ApplicantProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkills_Skills_SkillId",
                table: "ApplicantSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobPostings_JobPostingId",
                table: "Applications",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStatusLogs_UserAccounst_ChangedByUserId",
                table: "ApplicationStatusLogs",
                column: "ChangedByUserId",
                principalTable: "UserAccounst",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_UserAccounst_ApprovedByAdminId",
                table: "Companies",
                column: "ApprovedByAdminId",
                principalTable: "UserAccounst",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccounst_ApprovedByMemberId",
                table: "EmployerProfiles",
                column: "ApprovedByMemberId",
                principalTable: "UserAccounst",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccounst_UserAccountId",
                table: "EmployerProfiles",
                column: "UserAccountId",
                principalTable: "UserAccounst",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_Companies_CompanyId",
                table: "JobPostings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_JobClassifications_JobClassificationId",
                table: "JobPostings",
                column: "JobClassificationId",
                principalTable: "JobClassifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_JobSubClassifications_JobSubClassificationId",
                table: "JobPostings",
                column: "JobSubClassificationId",
                principalTable: "JobSubClassifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_UserAccounst_ApprovedByAdminId",
                table: "JobPostings",
                column: "ApprovedByAdminId",
                principalTable: "UserAccounst",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_UserAccounst_CreatedByEmployerId",
                table: "JobPostings",
                column: "CreatedByEmployerId",
                principalTable: "UserAccounst",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostingSkills_JobPostings_JobPostingId",
                table: "JobPostingSkills",
                column: "JobPostingId",
                principalTable: "JobPostings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSubClassifications_JobClassifications_JobClassificationId",
                table: "JobSubClassifications",
                column: "JobClassificationId",
                principalTable: "JobClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_JobSubClassifications_JobSubClassificationId",
                table: "Skills",
                column: "JobSubClassificationId",
                principalTable: "JobSubClassifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_UserAccounst_UserAccountId",
                table: "UserSessions",
                column: "UserAccountId",
                principalTable: "UserAccounst",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperiences_ApplicantProfiles_ApplicantProfileId",
                table: "WorkExperiences",
                column: "ApplicantProfileId",
                principalTable: "ApplicantProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantProfiles_UserAccounst_UserAccountId",
                table: "ApplicantProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkills_ApplicantProfiles_ApplicantProfileId",
                table: "ApplicantSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkills_Skills_SkillId",
                table: "ApplicantSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_Applications_JobPostings_JobPostingId",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStatusLogs_UserAccounst_ChangedByUserId",
                table: "ApplicationStatusLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_UserAccounst_ApprovedByAdminId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccounst_ApprovedByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccounst_UserAccountId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_Companies_CompanyId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_JobClassifications_JobClassificationId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_JobSubClassifications_JobSubClassificationId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_UserAccounst_ApprovedByAdminId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_UserAccounst_CreatedByEmployerId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostingSkills_JobPostings_JobPostingId",
                table: "JobPostingSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_JobSubClassifications_JobClassifications_JobClassificationId",
                table: "JobSubClassifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_JobSubClassifications_JobSubClassificationId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_UserAccounst_UserAccountId",
                table: "UserSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkExperiences_ApplicantProfiles_ApplicantProfileId",
                table: "WorkExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkExperiences",
                table: "WorkExperiences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounst",
                table: "UserAccounst");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobSubClassifications",
                table: "JobSubClassifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JobPostings",
                table: "JobPostings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicantSkills",
                table: "ApplicantSkills");

            migrationBuilder.RenameTable(
                name: "WorkExperiences",
                newName: "WorkExperience");

            migrationBuilder.RenameTable(
                name: "UserAccounst",
                newName: "UserAccount");

            migrationBuilder.RenameTable(
                name: "JobSubClassifications",
                newName: "JobSubClassification");

            migrationBuilder.RenameTable(
                name: "JobPostings",
                newName: "JobPosting");

            migrationBuilder.RenameTable(
                name: "ApplicantSkills",
                newName: "ApplicantSkill");

            migrationBuilder.RenameIndex(
                name: "IX_WorkExperiences_ApplicantProfileId",
                table: "WorkExperience",
                newName: "IX_WorkExperience_ApplicantProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounst_Email",
                table: "UserAccount",
                newName: "IX_UserAccount_Email");

            migrationBuilder.RenameIndex(
                name: "IX_JobSubClassifications_JobClassificationId_Name",
                table: "JobSubClassification",
                newName: "IX_JobSubClassification_JobClassificationId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_JobSubClassificationId",
                table: "JobPosting",
                newName: "IX_JobPosting_JobSubClassificationId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_JobClassificationId",
                table: "JobPosting",
                newName: "IX_JobPosting_JobClassificationId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_CreatedByEmployerId",
                table: "JobPosting",
                newName: "IX_JobPosting_CreatedByEmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_CompanyId",
                table: "JobPosting",
                newName: "IX_JobPosting_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_JobPostings_ApprovedByAdminId",
                table: "JobPosting",
                newName: "IX_JobPosting_ApprovedByAdminId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantSkills_SkillId",
                table: "ApplicantSkill",
                newName: "IX_ApplicantSkill_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicantSkills_ApplicantProfileId_SkillId",
                table: "ApplicantSkill",
                newName: "IX_ApplicantSkill_ApplicantProfileId_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkExperience",
                table: "WorkExperience",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccount",
                table: "UserAccount",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobSubClassification",
                table: "JobSubClassification",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JobPosting",
                table: "JobPosting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicantSkill",
                table: "ApplicantSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantProfiles_UserAccount_UserAccountId",
                table: "ApplicantProfiles",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkill_ApplicantProfiles_ApplicantProfileId",
                table: "ApplicantSkill",
                column: "ApplicantProfileId",
                principalTable: "ApplicantProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkill_Skills_SkillId",
                table: "ApplicantSkill",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_JobPosting_JobPostingId",
                table: "Applications",
                column: "JobPostingId",
                principalTable: "JobPosting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStatusLogs_UserAccount_ChangedByUserId",
                table: "ApplicationStatusLogs",
                column: "ChangedByUserId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_UserAccount_ApprovedByAdminId",
                table: "Companies",
                column: "ApprovedByAdminId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccount_ApprovedByMemberId",
                table: "EmployerProfiles",
                column: "ApprovedByMemberId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccount_UserAccountId",
                table: "EmployerProfiles",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosting_Companies_CompanyId",
                table: "JobPosting",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosting_JobClassifications_JobClassificationId",
                table: "JobPosting",
                column: "JobClassificationId",
                principalTable: "JobClassifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosting_JobSubClassification_JobSubClassificationId",
                table: "JobPosting",
                column: "JobSubClassificationId",
                principalTable: "JobSubClassification",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosting_UserAccount_ApprovedByAdminId",
                table: "JobPosting",
                column: "ApprovedByAdminId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPosting_UserAccount_CreatedByEmployerId",
                table: "JobPosting",
                column: "CreatedByEmployerId",
                principalTable: "UserAccount",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostingSkills_JobPosting_JobPostingId",
                table: "JobPostingSkills",
                column: "JobPostingId",
                principalTable: "JobPosting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobSubClassification_JobClassifications_JobClassificationId",
                table: "JobSubClassification",
                column: "JobClassificationId",
                principalTable: "JobClassifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_JobSubClassification_JobSubClassificationId",
                table: "Skills",
                column: "JobSubClassificationId",
                principalTable: "JobSubClassification",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_UserAccount_UserAccountId",
                table: "UserSessions",
                column: "UserAccountId",
                principalTable: "UserAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkExperience_ApplicantProfiles_ApplicantProfileId",
                table: "WorkExperience",
                column: "ApplicantProfileId",
                principalTable: "ApplicantProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
