using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMarriageFormSectionRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CurrentStage",
                table: "MarriageApplicationForms",
                newName: "ApplicationStage");

            migrationBuilder.AddColumn<int>(
                name: "FormStage",
                table: "MarriageApplicationForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "MarriageApplicationFormId",
                table: "BrideGrooms",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BrideGuardian",
                columns: table => new
                {
                    BrideGuardianId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    GuardianName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, defaultValue: ""),
                    GuardianRelationToBride = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValue: ""),
                    GuardianAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false, defaultValue: ""),
                    GuardianTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false, defaultValue: ""),
                    GuardianSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: ""),
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrideGuardian", x => x.BrideGuardianId);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AmirApprovalSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmirApprovalSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmirApprovalSection_MarriageApplicationForms_MarriageApplica~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BrideFormSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrideFormSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrideFormSection_MarriageApplicationForms_MarriageApplicatio~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GuardianOrWakeelSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    PartyType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    Tel = table.Column<string>(type: "longtext", nullable: false),
                    RelationToBride = table.Column<string>(type: "longtext", nullable: false),
                    ActingFor = table.Column<string>(type: "longtext", nullable: true),
                    Signature = table.Column<string>(type: "longtext", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    JamaatMemberId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuardianOrWakeelSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuardianOrWakeelSection_JamaatMembers_JamaatMemberId",
                        column: x => x.JamaatMemberId,
                        principalTable: "JamaatMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GuardianOrWakeelSection_MarriageApplicationForms_MarriageApp~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImamVerificationSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImamVerificationSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImamVerificationSection_MarriageApplicationForms_MarriageApp~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JamaatPresidentVerificationSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JamaatPresidentVerificationSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JamaatPresidentVerificationSection_MarriageApplicationForms_~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RishtanataRecommendationSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RishtanataRecommendationSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RishtanataRecommendationSection_MarriageApplicationForms_Mar~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WitnessSignatureSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WitnessSignatureSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WitnessSignatureSection_MarriageApplicationForms_MarriageApp~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_BrideGrooms_MarriageApplicationFormId",
                table: "BrideGrooms",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AmirApprovalSection_MarriageApplicationFormId",
                table: "AmirApprovalSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrideFormSection_MarriageApplicationFormId",
                table: "BrideFormSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuardianOrWakeelSection_JamaatMemberId",
                table: "GuardianOrWakeelSection",
                column: "JamaatMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_GuardianOrWakeelSection_MarriageApplicationFormId",
                table: "GuardianOrWakeelSection",
                column: "MarriageApplicationFormId",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_BrideGuardian_MarriageApplicationId",
                table: "BrideGuardian",
                column: "MarriageApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImamVerificationSection_MarriageApplicationFormId",
                table: "ImamVerificationSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JamaatPresidentVerificationSection_MarriageApplicationFormId",
                table: "JamaatPresidentVerificationSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RishtanataRecommendationSection_MarriageApplicationFormId",
                table: "RishtanataRecommendationSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WitnessSignatureSection_MarriageApplicationFormId",
                table: "WitnessSignatureSection",
                column: "MarriageApplicationFormId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrideGrooms_MarriageApplicationForms_MarriageApplicationForm~",
                table: "BrideGrooms",
                column: "MarriageApplicationFormId",
                principalTable: "MarriageApplicationForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrideGuardian_MarriageApplicationForms_MarriageApplicationId",
                table: "BrideGuardian",
                column: "MarriageApplicationId",
                principalTable: "MarriageApplicationForms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrideGrooms_MarriageApplicationForms_MarriageApplicationForm~",
                table: "BrideGrooms");

            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_BrideGuardian_MarriageApplicationForms_MarriageApplicationId",
                table: "BrideGuardian");

            migrationBuilder.DropTable(
                name: "AmirApprovalSection");

            migrationBuilder.DropTable(
                name: "BrideFormSection");

            migrationBuilder.DropTable(
                name: "GuardianOrWakeelSection");

            migrationBuilder.DropTable(
                name: "ImamVerificationSection");

            migrationBuilder.DropTable(
                name: "JamaatPresidentVerificationSection");

            migrationBuilder.DropTable(
                name: "RishtanataRecommendationSection");

            migrationBuilder.DropTable(
                name: "WitnessSignatureSection");

            migrationBuilder.DropTable(
                name: "BrideGuardian");

            migrationBuilder.DropIndex(
                name: "IX_BrideGrooms_MarriageApplicationFormId",
                table: "BrideGrooms");

            migrationBuilder.DropColumn(
                name: "FormStage",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "MarriageApplicationFormId",
                table: "BrideGrooms");

            migrationBuilder.RenameColumn(
                name: "ApplicationStage",
                table: "MarriageApplicationForms",
                newName: "CurrentStage");
        }
    }
}
