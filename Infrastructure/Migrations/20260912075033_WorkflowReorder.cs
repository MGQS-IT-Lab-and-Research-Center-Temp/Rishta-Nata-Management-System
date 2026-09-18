using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WorkflowReorder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OfficiatingImamMembershipNo",
                table: "RishtanataRecommendations",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroomJamaatPresidentName",
                table: "NikahApplications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroomJamaatPresidentSignatureDate",
                table: "NikahApplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OfficiatingImamMembershipNo",
                table: "NikahApplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "GroomJamaatPresidentVerifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Tel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    SignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroomJamaatPresidentVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroomJamaatPresidentVerifications_NikahApplications_Marriage~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_GroomJamaatPresidentVerifications_MarriageApplicationFormId",
                table: "GroomJamaatPresidentVerifications",
                column: "MarriageApplicationFormId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroomJamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "OfficiatingImamMembershipNo",
                table: "RishtanataRecommendations");

            migrationBuilder.DropColumn(
                name: "GroomJamaatPresidentName",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "GroomJamaatPresidentSignatureDate",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "OfficiatingImamMembershipNo",
                table: "NikahApplications");
        }
    }
}
