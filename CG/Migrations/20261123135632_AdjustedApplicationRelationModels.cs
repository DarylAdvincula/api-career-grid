using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedApplicationRelationModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkills_Skills_SkillId",
                table: "ApplicantSkills");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkills_Skills_SkillId",
                table: "ApplicantSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkills_Skills_SkillId",
                table: "ApplicantSkills");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkills_Skills_SkillId",
                table: "ApplicantSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
