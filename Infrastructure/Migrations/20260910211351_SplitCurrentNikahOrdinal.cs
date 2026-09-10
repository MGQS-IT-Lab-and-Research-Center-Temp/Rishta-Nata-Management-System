using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitCurrentNikahOrdinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSecondThirdOrFourthNikah",
                table: "NikahGrooms");

            migrationBuilder.DropColumn(
                name: "IsSecondThirdOrFourthNikah",
                table: "NikahApplications");

            migrationBuilder.AddColumn<int>(
                name: "CurrentNikahOrdinal",
                table: "NikahGrooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentNikahOrdinal",
                table: "NikahApplications",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentNikahOrdinal",
                table: "NikahGrooms");

            migrationBuilder.DropColumn(
                name: "CurrentNikahOrdinal",
                table: "NikahApplications");

            migrationBuilder.AddColumn<bool>(
                name: "IsSecondThirdOrFourthNikah",
                table: "NikahGrooms",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSecondThirdOrFourthNikah",
                table: "NikahApplications",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
