using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeBrideGuardianRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardians_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrideGuardianId",
                table: "JamaatMembers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_BrideGuardians_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardians",
                principalColumn: "BrideGuardianId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardians_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrideGuardianId",
                table: "JamaatMembers",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_BrideGuardians_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardians",
                principalColumn: "BrideGuardianId",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
