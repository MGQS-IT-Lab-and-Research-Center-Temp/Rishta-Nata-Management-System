using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentStageAndSyncPendingModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentStage",
                table: "MarriageApplicationForms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "BrideGrooms",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "MarriageFormRejections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RejectedAtStage = table.Column<int>(type: "int", nullable: false),
                    RevertedToStage = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarriageFormRejections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarriageFormRejections_MarriageApplicationForms_MarriageAppl~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Comment = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_FormApplications_MarriageApplicationId",
                        column: x => x.MarriageApplicationId,
                        principalTable: "FormApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MarriageFormRejections_MarriageApplicationFormId",
                table: "MarriageFormRejections",
                column: "MarriageApplicationFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MarriageApplicationId",
                table: "Reviews",
                column: "MarriageApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarriageFormRejections");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropColumn(
                name: "CurrentStage",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "BrideGrooms");
        }
    }
}
