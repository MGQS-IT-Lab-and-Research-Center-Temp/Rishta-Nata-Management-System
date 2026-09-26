using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameRishtanataRecommendationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // The Oracle MySql.EntityFrameworkCore provider's RenameColumn implementation
            // emits `CHANGE COLUMN old new longtext NOT NULL DEFAULT ''`, and MySQL rejects
            // a literal DEFAULT on a TEXT/BLOB column ("BLOB, TEXT, GEOMETRY or JSON column
            // 'Name' can't have a default value"). Native MySQL RENAME COLUMN performs the
            // same data-preserving rename without restating type/default, so it is used here
            // instead of migrationBuilder.RenameColumn to avoid that provider bug.
            migrationBuilder.Sql(
                "ALTER TABLE `RishtanataRecommendations` RENAME COLUMN `WakeelName` TO `Name`;");

            migrationBuilder.Sql(
                "ALTER TABLE `RishtanataRecommendations` RENAME COLUMN `WakeelDeclaration` TO `Recommendation`;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ALTER TABLE `RishtanataRecommendations` RENAME COLUMN `Name` TO `WakeelName`;");

            migrationBuilder.Sql(
                "ALTER TABLE `RishtanataRecommendations` RENAME COLUMN `Recommendation` TO `WakeelDeclaration`;");
        }
    }
}
