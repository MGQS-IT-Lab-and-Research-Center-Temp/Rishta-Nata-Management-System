using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWitness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WitnessOneAddress",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessOneName",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessOneSignatureDate",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessOneTel",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessTwoAddress",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessTwoName",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessTwoSignatureDate",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessTwoTel",
                table: "MarriageApplicationForms");

            migrationBuilder.CreateTable(
                name: "Witnesses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    FullName = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    PhoneNumber = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    SignatureDate = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    WitnessNumber = table.Column<int>(type: "int", nullable: false),
                    InvitationToken = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false),
                    IsCompleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Witnesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Witnesses_MarriageApplicationForms_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Witnesses_InvitationToken",
                table: "Witnesses",
                column: "InvitationToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Witnesses_MarriageApplicationFormId_Role_WitnessNumber",
                table: "Witnesses",
                columns: new[] { "MarriageApplicationFormId", "Role", "WitnessNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Witnesses");

            migrationBuilder.AddColumn<string>(
                name: "WitnessOneAddress",
                table: "MarriageApplicationForms",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessOneName",
                table: "MarriageApplicationForms",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessOneSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessOneTel",
                table: "MarriageApplicationForms",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessTwoAddress",
                table: "MarriageApplicationForms",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessTwoName",
                table: "MarriageApplicationForms",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessTwoSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessTwoTel",
                table: "MarriageApplicationForms",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
