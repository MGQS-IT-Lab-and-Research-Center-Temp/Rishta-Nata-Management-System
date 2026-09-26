using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGuardianRepresentative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AppointsRepresentative",
                table: "NikahGuardians",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RepresentativeAddress",
                table: "NikahGuardians",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "RepresentativeDate",
                table: "NikahGuardians",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RepresentativeName",
                table: "NikahGuardians",
                type: "longtext",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "RepresentativeSignature",
                table: "NikahGuardians",
                type: "longtext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointsRepresentative",
                table: "NikahGuardians");

            migrationBuilder.DropColumn(
                name: "RepresentativeAddress",
                table: "NikahGuardians");

            migrationBuilder.DropColumn(
                name: "RepresentativeDate",
                table: "NikahGuardians");

            migrationBuilder.DropColumn(
                name: "RepresentativeName",
                table: "NikahGuardians");

            migrationBuilder.DropColumn(
                name: "RepresentativeSignature",
                table: "NikahGuardians");
        }
    }
}
