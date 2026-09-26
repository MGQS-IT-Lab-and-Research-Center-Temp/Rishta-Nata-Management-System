using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGroomWakeelSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing rows are groom sections whose groom attends in person
            // (domain default true).
            migrationBuilder.AddColumn<bool>(
                name: "CanAttendNikahInPerson",
                table: "NikahGrooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "WakeelName",
                table: "NikahGrooms",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WakeelTel",
                table: "NikahGrooms",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "CanAttendNikahInPerson",
                table: "NikahApplications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "GroomWakeelFatherName",
                table: "NikahApplications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroomWakeelName",
                table: "NikahApplications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroomWakeelSignatureDate",
                table: "NikahApplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroomWakeelTel",
                table: "NikahApplications",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WakeelName",
                table: "NikahApplications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WakeelTel",
                table: "NikahApplications",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NikahGroomWakeels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    FatherName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Tel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    Signature = table.Column<string>(type: "longtext", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NikahGroomWakeels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NikahGroomWakeels_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_NikahGroomWakeels_MarriageApplicationFormId",
                table: "NikahGroomWakeels",
                column: "MarriageApplicationFormId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NikahGroomWakeels");

            migrationBuilder.DropColumn(
                name: "CanAttendNikahInPerson",
                table: "NikahGrooms");

            migrationBuilder.DropColumn(
                name: "WakeelName",
                table: "NikahGrooms");

            migrationBuilder.DropColumn(
                name: "WakeelTel",
                table: "NikahGrooms");

            migrationBuilder.DropColumn(
                name: "CanAttendNikahInPerson",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "GroomWakeelFatherName",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "GroomWakeelName",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "GroomWakeelSignatureDate",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "GroomWakeelTel",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "WakeelName",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "WakeelTel",
                table: "NikahApplications");
        }
    }
}
