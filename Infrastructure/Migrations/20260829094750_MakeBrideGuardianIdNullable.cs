using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeBrideGuardianIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrideGuardianId",
                table: "JamaatMembers",
                type: "char(36)",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardian",
                principalColumn: "BrideGuardianId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AlterColumn<decimal>(
                name: "DowryAmount",
                table: "Certificates",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BridegroomDowerAmountToBePaid",
                table: "BrideGrooms",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BridegroomDowerAmountPaidInCash",
                table: "BrideGrooms",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.AlterColumn<Guid>(
                name: "BrideGuardianId",
                table: "JamaatMembers",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardian",
                principalColumn: "BrideGuardianId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AlterColumn<decimal>(
                name: "DowryAmount",
                table: "Certificates",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BridegroomDowerAmountToBePaid",
                table: "BrideGrooms",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BridegroomDowerAmountPaidInCash",
                table: "BrideGrooms",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");
        }
    }
}