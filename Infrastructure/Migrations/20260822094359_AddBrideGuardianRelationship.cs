using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrideGuardianRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BrideGuardianId",
                table: "JamaatMembers",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrideGuardians",
                columns: table => new
                {
                    BrideGuardianId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    GuardianName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    GuardianRelationToBride = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    GuardianAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    GuardianTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    GuardianSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrideGuardians", x => x.BrideGuardianId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Token = table.Column<string>(type: "longtext", nullable: false),
                    TargetType = table.Column<int>(type: "int", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: true),
                    MarriageReferenceNumber = table.Column<string>(type: "longtext", nullable: true),
                    RecipientJamaatMemberId = table.Column<Guid>(type: "char(36)", nullable: true),
                    RecipientMembershipNo = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Used = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JamaatMembers_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId");

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_BrideGuardians_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardians",
                principalColumn: "BrideGuardianId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardians_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.DropTable(
                name: "BrideGuardians");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_JamaatMembers_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.DropColumn(
                name: "BrideGuardianId",
                table: "JamaatMembers");
        }
    }
}
