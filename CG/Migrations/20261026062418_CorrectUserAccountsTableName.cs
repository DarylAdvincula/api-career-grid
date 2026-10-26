using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class CorrectUserAccountsTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantProfiles_UserAccounst_UserAccountId",
                table: "ApplicantProfiles");

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
                name: "FK_JobPostings_UserAccounst_ApprovedByAdminId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_UserAccounst_CreatedByEmployerId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_UserAccounst_UserAccountId",
                table: "UserSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounst",
                table: "UserAccounst");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "UserAccounst",
                newName: "UserAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounst_Email",
                table: "UserAccounts",
                newName: "IX_UserAccounts_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantProfiles_UserAccounts_UserAccountId",
                table: "ApplicantProfiles",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStatusLogs_UserAccounts_ChangedByUserId",
                table: "ApplicationStatusLogs",
                column: "ChangedByUserId",
                principalTable: "UserAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_UserAccounts_ApprovedByAdminId",
                table: "Companies",
                column: "ApprovedByAdminId",
                principalTable: "UserAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccounts_ApprovedByMemberId",
                table: "EmployerProfiles",
                column: "ApprovedByMemberId",
                principalTable: "UserAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccounts_UserAccountId",
                table: "EmployerProfiles",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_UserAccounts_ApprovedByAdminId",
                table: "JobPostings",
                column: "ApprovedByAdminId",
                principalTable: "UserAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JobPostings_UserAccounts_CreatedByEmployerId",
                table: "JobPostings",
                column: "CreatedByEmployerId",
                principalTable: "UserAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSessions_UserAccounts_UserAccountId",
                table: "UserSessions",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantProfiles_UserAccounts_UserAccountId",
                table: "ApplicantProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStatusLogs_UserAccounts_ChangedByUserId",
                table: "ApplicationStatusLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_UserAccounts_ApprovedByAdminId",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccounts_ApprovedByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccounts_UserAccountId",
                table: "EmployerProfiles");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_UserAccounts_ApprovedByAdminId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobPostings_UserAccounts_CreatedByEmployerId",
                table: "JobPostings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSessions_UserAccounts_UserAccountId",
                table: "UserSessions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                newName: "UserAccounst");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_Email",
                table: "UserAccounst",
                newName: "IX_UserAccounst_Email");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Companies",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounst",
                table: "UserAccounst",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantProfiles_UserAccounst_UserAccountId",
                table: "ApplicantProfiles",
                column: "UserAccountId",
                principalTable: "UserAccounst",
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
                name: "FK_UserSessions_UserAccounst_UserAccountId",
                table: "UserSessions",
                column: "UserAccountId",
                principalTable: "UserAccounst",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
