using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncFormStageAndVerificationApprovalSections : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_BrideGuardian_TempId1",
                table: "BrideGuardian");

            migrationBuilder.RenameColumn(
                name: "CurrentStage",
                table: "MarriageApplicationForms",
                newName: "ApplicationStage");

            migrationBuilder.RenameColumn(
                name: "TempId1",
                table: "BrideGuardian",
                newName: "MarriageApplicationId");

            migrationBuilder.AddColumn<Guid>(
                name: "BridegroomSectionId",
                table: "MarriageApplicationForms",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FormStage",
                table: "MarriageApplicationForms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "BrideGuardianId",
                table: "BrideGuardian",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "BrideGuardian",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "BrideGuardian",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuardianAddress",
                table: "BrideGuardian",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuardianName",
                table: "BrideGuardian",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuardianRelationToBride",
                table: "BrideGuardian",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuardianSignatureDate",
                table: "BrideGuardian",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GuardianTel",
                table: "BrideGuardian",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "BrideGuardian",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "BrideGuardian",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "BrideGuardian",
                type: "char(36)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "BrideGuardian",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BrideGuardian",
                table: "BrideGuardian",
                column: "BrideGuardianId");

            migrationBuilder.CreateTable(
                name: "AmirApprovalSection",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApprovedDateOfNikah = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SignatureDate = table.Column<string>(type: "longtext", nullable: false),
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
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    AddressJamaat = table.Column<string>(type: "longtext", nullable: false),
                    Tel = table.Column<string>(type: "longtext", nullable: false),
                    SignatureDate = table.Column<string>(type: "longtext", nullable: false),
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
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Tel = table.Column<string>(type: "longtext", nullable: false),
                    SignatureDate = table.Column<string>(type: "longtext", nullable: false),
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
                    WakeelName = table.Column<string>(type: "longtext", nullable: false),
                    WakeelDeclaration = table.Column<string>(type: "longtext", nullable: false),
                    SignatureDate = table.Column<string>(type: "longtext", nullable: false),
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
                name: "IX_MarriageApplicationForms_BridegroomSectionId",
                table: "MarriageApplicationForms",
                column: "BridegroomSectionId");

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
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardian",
                principalColumn: "BrideGuardianId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MarriageApplicationForms_BrideGrooms_BridegroomSectionId",
                table: "MarriageApplicationForms",
                column: "BridegroomSectionId",
                principalTable: "BrideGrooms",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_MarriageApplicationForms_BrideGrooms_BridegroomSectionId",
                table: "MarriageApplicationForms");

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

            migrationBuilder.DropIndex(
                name: "IX_MarriageApplicationForms_BridegroomSectionId",
                table: "MarriageApplicationForms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BrideGuardian",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "BridegroomSectionId",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "FormStage",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideGuardianId",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "GuardianAddress",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "GuardianName",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "GuardianRelationToBride",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "GuardianSignatureDate",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "GuardianTel",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "BrideGuardian");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "BrideGuardian");

            migrationBuilder.RenameColumn(
                name: "ApplicationStage",
                table: "MarriageApplicationForms",
                newName: "CurrentStage");

            migrationBuilder.RenameColumn(
                name: "MarriageApplicationId",
                table: "BrideGuardian",
                newName: "TempId1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_BrideGuardian_TempId1",
                table: "BrideGuardian",
                column: "TempId1");

            migrationBuilder.AddForeignKey(
                name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId",
                principalTable: "BrideGuardian",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
