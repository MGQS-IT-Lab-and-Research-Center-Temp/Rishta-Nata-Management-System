using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJamaatMemberAndRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JamaatRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: false),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JamaatRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JamaatMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Surname = table.Column<string>(type: "longtext", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    ChandaNo = table.Column<string>(type: "longtext", nullable: false),
                    WasiyatNo = table.Column<string>(type: "longtext", nullable: true),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    AuxillaryBodyName = table.Column<string>(type: "longtext", nullable: true),
                    MiddleName = table.Column<string>(type: "longtext", nullable: true),
                    MaidenName = table.Column<string>(type: "longtext", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneNo = table.Column<string>(type: "longtext", nullable: true),
                    JamaatName = table.Column<string>(type: "longtext", nullable: false),
                    CircuitName = table.Column<string>(type: "longtext", nullable: false),
                    Sex = table.Column<string>(type: "longtext", nullable: false),
                    MaritalStatus = table.Column<string>(type: "longtext", nullable: true),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    NextOfKinPhoneNo = table.Column<string>(type: "longtext", nullable: true),
                    NextOfKinName = table.Column<string>(type: "longtext", nullable: true),
                    NextOfKinAddress = table.Column<string>(type: "longtext", nullable: true),
                    Nationality = table.Column<string>(type: "longtext", nullable: true),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsSystemDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NewRole = table.Column<string>(type: "longtext", nullable: false),
                    ResetToken = table.Column<string>(type: "longtext", nullable: true),
                    ResetTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JamaatMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JamaatMembers_JamaatRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "JamaatRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_JamaatMembers_RoleId",
                table: "JamaatMembers",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JamaatMembers");

            migrationBuilder.DropTable(
                name: "JamaatRoles");
        }
    }
}
