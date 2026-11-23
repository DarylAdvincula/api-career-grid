using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class AddCodeColumnInCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Companies",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Code",
                table: "Companies",
                column: "Code",
                unique: true,
                filter: "[Code] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Companies_Code",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Companies");
        }
    }
}
