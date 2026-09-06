using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAqeeqahCertificates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AqeeqahCertificates");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AqeeqahCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    AnimalCount = table.Column<int>(type: "int", nullable: false),
                    AqeeqahDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AqeeqahLocation = table.Column<string>(type: "longtext", nullable: false),
                    CertificateFilePath = table.Column<string>(type: "longtext", nullable: true),
                    ChildName = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FatherName = table.Column<string>(type: "longtext", nullable: false),
                    Gender = table.Column<string>(type: "longtext", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IssuedByUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    JamaatId = table.Column<Guid>(type: "char(36)", nullable: false),
                    JamaatName = table.Column<string>(type: "longtext", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    MotherName = table.Column<string>(type: "longtext", nullable: false),
                    OfficiatingMissionary = table.Column<string>(type: "longtext", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "longtext", nullable: false),
                    SerialNumber = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AqeeqahCertificates", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }
    }
}
