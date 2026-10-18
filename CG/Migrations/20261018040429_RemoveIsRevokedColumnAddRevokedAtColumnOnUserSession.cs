using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CG.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsRevokedColumnAddRevokedAtColumnOnUserSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "UserSessions",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAt",
                table: "UserSessions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RevokedAt",
                table: "UserSessions");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "UserSessions",
                newName: "CreateAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "UserSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
