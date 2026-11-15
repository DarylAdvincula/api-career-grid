using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDraftStepAndIsSubmittedFieldsInApplicationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DraftStep",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "IsSubmitted",
                table: "Applications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DraftStep",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubmitted",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
