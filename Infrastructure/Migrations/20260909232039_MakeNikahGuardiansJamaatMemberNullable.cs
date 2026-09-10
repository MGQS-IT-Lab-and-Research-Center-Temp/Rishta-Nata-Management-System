using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeNikahGuardiansJamaatMemberNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NikahGuardians_JamaatMembers_JamaatMemberId",
                table: "NikahGuardians");

            migrationBuilder.AlterColumn<Guid>(
                name: "JamaatMemberId",
                table: "NikahGuardians",
                type: "char(36)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "char(36)");

            migrationBuilder.AddForeignKey(
                name: "FK_NikahGuardians_JamaatMembers_JamaatMemberId",
                table: "NikahGuardians",
                column: "JamaatMemberId",
                principalTable: "JamaatMembers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NikahGuardians_JamaatMembers_JamaatMemberId",
                table: "NikahGuardians");

            migrationBuilder.AlterColumn<Guid>(
                name: "JamaatMemberId",
                table: "NikahGuardians",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NikahGuardians_JamaatMembers_JamaatMemberId",
                table: "NikahGuardians",
                column: "JamaatMemberId",
                principalTable: "JamaatMembers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
