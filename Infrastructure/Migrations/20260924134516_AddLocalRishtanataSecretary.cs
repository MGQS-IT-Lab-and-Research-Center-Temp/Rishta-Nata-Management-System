using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalRishtanataSecretary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrideLocalRishtanataSecretaryName",
                table: "NikahApplications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrideLocalRishtanataSecretarySignatureDate",
                table: "NikahApplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroomLocalRishtanataSecretaryName",
                table: "NikahApplications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GroomLocalRishtanataSecretarySignatureDate",
                table: "NikahApplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocalRishtanataSecretaryName",
                table: "JamaatPresidentVerifications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocalRishtanataSecretarySignatureDate",
                table: "JamaatPresidentVerifications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocalRishtanataSecretaryTel",
                table: "JamaatPresidentVerifications",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocalRishtanataSecretaryName",
                table: "GroomJamaatPresidentVerifications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocalRishtanataSecretarySignatureDate",
                table: "GroomJamaatPresidentVerifications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LocalRishtanataSecretaryTel",
                table: "GroomJamaatPresidentVerifications",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrideLocalRishtanataSecretaryName",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "BrideLocalRishtanataSecretarySignatureDate",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "GroomLocalRishtanataSecretaryName",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "GroomLocalRishtanataSecretarySignatureDate",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "LocalRishtanataSecretaryName",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "LocalRishtanataSecretarySignatureDate",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "LocalRishtanataSecretaryTel",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "LocalRishtanataSecretaryName",
                table: "GroomJamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "LocalRishtanataSecretarySignatureDate",
                table: "GroomJamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "LocalRishtanataSecretaryTel",
                table: "GroomJamaatPresidentVerifications");
        }
    }
}
