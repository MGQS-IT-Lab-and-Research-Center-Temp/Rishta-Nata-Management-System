using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJamaatPresidentAttestations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BrideIsBornAhmadi",
                table: "JamaatPresidentVerifications",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrideMarriageReason",
                table: "JamaatPresidentVerifications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "BrideSignedFreely",
                table: "JamaatPresidentVerifications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BrideYearsAsAhmadi",
                table: "JamaatPresidentVerifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GroomIsBornAhmadi",
                table: "JamaatPresidentVerifications",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroomMarriageReason",
                table: "JamaatPresidentVerifications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GroomYearsAsAhmadi",
                table: "JamaatPresidentVerifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GuardianIsBonafide",
                table: "JamaatPresidentVerifications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GroomIsBornAhmadi",
                table: "GroomJamaatPresidentVerifications",
                type: "tinyint(1)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GroomMarriageReason",
                table: "GroomJamaatPresidentVerifications",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "GroomYearsAsAhmadi",
                table: "GroomJamaatPresidentVerifications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrideIsBornAhmadi",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "BrideMarriageReason",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "BrideSignedFreely",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "BrideYearsAsAhmadi",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "GroomIsBornAhmadi",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "GroomMarriageReason",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "GroomYearsAsAhmadi",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "GuardianIsBonafide",
                table: "JamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "GroomIsBornAhmadi",
                table: "GroomJamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "GroomMarriageReason",
                table: "GroomJamaatPresidentVerifications");

            migrationBuilder.DropColumn(
                name: "GroomYearsAsAhmadi",
                table: "GroomJamaatPresidentVerifications");
        }
    }
}
