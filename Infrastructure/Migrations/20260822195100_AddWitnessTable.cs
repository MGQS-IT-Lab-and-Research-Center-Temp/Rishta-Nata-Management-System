using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWitnessTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_JamaatRoles_RoleId",
                table: "JamaatMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_JamaatRoles",
                table: "JamaatRoles");

            migrationBuilder.RenameTable(
                name: "JamaatRoles",
                newName: "Role");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Role",
                table: "Role",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_Role_RoleId",
                table: "JamaatMembers",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_Role_RoleId",
                table: "JamaatMembers");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Role",
                table: "Role");

            migrationBuilder.RenameTable(
                name: "Role",
                newName: "JamaatRoles");

            migrationBuilder.AddPrimaryKey(
                name: "PK_JamaatRoles",
                table: "JamaatRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_JamaatRoles_RoleId",
                table: "JamaatMembers",
                column: "RoleId",
                principalTable: "JamaatRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
