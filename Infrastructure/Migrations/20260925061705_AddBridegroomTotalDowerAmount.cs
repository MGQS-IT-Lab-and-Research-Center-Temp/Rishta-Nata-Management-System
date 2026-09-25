using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBridegroomTotalDowerAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "BridegroomTotalDowerAmount",
                table: "NikahGrooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "BridegroomTotalDowerAmount",
                table: "NikahApplications",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            // Gap 9: existing rows get Total = PaidInCash + ToBePaid, so they
            // satisfy BridegroomDowerRules as soon as the column exists.
            migrationBuilder.Sql(
                "UPDATE `NikahApplications` SET `BridegroomTotalDowerAmount` = " +
                "`BridegroomDowerAmountPaidInCash` + `BridegroomDowerAmountToBePaid`;");

            migrationBuilder.Sql(
                "UPDATE `NikahGrooms` SET `BridegroomTotalDowerAmount` = " +
                "`BridegroomDowerAmountPaidInCash` + `BridegroomDowerAmountToBePaid`;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BridegroomTotalDowerAmount",
                table: "NikahGrooms");

            migrationBuilder.DropColumn(
                name: "BridegroomTotalDowerAmount",
                table: "NikahApplications");
        }
    }
}
