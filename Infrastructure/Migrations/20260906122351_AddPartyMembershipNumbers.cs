using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPartyMembershipNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrideFatherMembershipNo",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BridegroomFatherMembershipNo",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessOneMembershipNo",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessTwoMembershipNo",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrideFatherMembershipNo",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BridegroomFatherMembershipNo",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessOneMembershipNo",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "WitnessTwoMembershipNo",
                table: "MarriageApplicationForms");
        }
    }
}
