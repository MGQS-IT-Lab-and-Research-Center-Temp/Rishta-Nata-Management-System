using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrideGuardianLinkToJamaatMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrideGuardianId",
                table: "JamaatMembers",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_JamaatMembers_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId");

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardian",
                principalColumn: "BrideGuardianId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.DropIndex(
                name: "IX_JamaatMembers_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.DropColumn(
                name: "BrideGuardianId",
                table: "JamaatMembers");
        }
    }
}