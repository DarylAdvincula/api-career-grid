using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class AddModelAdjustmentsForServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccount_ApproverByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployerProfiles_ApproverByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.DropColumn(
                name: "ApproverByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.AlterColumn<long>(
                name: "FileSizeInBytes",
                table: "Resumes",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ApprovedByMemberId",
                table: "EmployerProfiles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_ApprovedByMemberId",
                table: "EmployerProfiles",
                column: "ApprovedByMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccount_ApprovedByMemberId",
                table: "EmployerProfiles",
                column: "ApprovedByMemberId",
                principalTable: "UserAccount",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployerProfiles_UserAccount_ApprovedByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.DropIndex(
                name: "IX_EmployerProfiles_ApprovedByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.DropColumn(
                name: "ApprovedByMemberId",
                table: "EmployerProfiles");

            migrationBuilder.AlterColumn<int>(
                name: "FileSizeInBytes",
                table: "Resumes",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "ApproverByMemberId",
                table: "EmployerProfiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EmployerProfiles_ApproverByMemberId",
                table: "EmployerProfiles",
                column: "ApproverByMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployerProfiles_UserAccount_ApproverByMemberId",
                table: "EmployerProfiles",
                column: "ApproverByMemberId",
                principalTable: "UserAccount",
                principalColumn: "Id");
        }
    }
}
