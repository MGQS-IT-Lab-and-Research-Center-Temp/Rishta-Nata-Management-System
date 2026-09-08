using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDivorceEvidence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BridegroomDivorceEvidence",
                table: "NikahGrooms",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrideDivorceEvidence",
                table: "NikahBrides",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrideDivorceEvidence",
                table: "NikahApplications",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BridegroomDivorceEvidence",
                table: "NikahApplications",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BridegroomDivorceEvidence",
                table: "NikahGrooms");

            migrationBuilder.DropColumn(
                name: "BrideDivorceEvidence",
                table: "NikahBrides");

            migrationBuilder.DropColumn(
                name: "BrideDivorceEvidence",
                table: "NikahApplications");

            migrationBuilder.DropColumn(
                name: "BridegroomDivorceEvidence",
                table: "NikahApplications");
        }
    }
}
