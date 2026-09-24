using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class BrideMaritalStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BrideMaritalStatus",
                table: "NikahBrides",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "BrideMaritalStatus",
                table: "NikahApplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            // Gap 6: legacy free-text values → enum names; anything unrecognised
            // (including '') becomes NULL so the bride must choose again.
            foreach (var table in new[] { "NikahApplications", "NikahBrides" })
            {
                migrationBuilder.Sql($@"
UPDATE `{table}` SET `BrideMaritalStatus` =
    CASE LOWER(TRIM(`BrideMaritalStatus`))
        WHEN 'single' THEN 'Unmarried'
        WHEN 'unmarried' THEN 'Unmarried'
        WHEN 'widowed' THEN 'WidowedIddatComplete'
        WHEN 'widowediddatcomplete' THEN 'WidowedIddatComplete'
        WHEN 'divorced' THEN 'DivorcedIddatComplete'
        WHEN 'divorcediddatcomplete' THEN 'DivorcedIddatComplete'
        ELSE NULL
    END;");
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            foreach (var table in new[] { "NikahApplications", "NikahBrides" })
            {
                migrationBuilder.Sql($@"
UPDATE `{table}` SET `BrideMaritalStatus` =
    CASE `BrideMaritalStatus`
        WHEN 'Unmarried' THEN 'Single'
        WHEN 'WidowedIddatComplete' THEN 'Widowed'
        WHEN 'DivorcedIddatComplete' THEN 'Divorced'
        ELSE ''
    END;");
            }

            migrationBuilder.AlterColumn<string>(
                name: "BrideMaritalStatus",
                table: "NikahBrides",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BrideMaritalStatus",
                table: "NikahApplications",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
