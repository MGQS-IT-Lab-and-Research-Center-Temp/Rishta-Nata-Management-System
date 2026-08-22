using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrideTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrideBloodGroup",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideDateOfBirth",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideDowerAmountReceivedInCash",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideGenotype",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideMaritalStatus",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideMembershipNo",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideName",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideProposedDowerAmount",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideResidentOf",
                table: "MarriageApplicationForms");

            migrationBuilder.DropColumn(
                name: "BrideSignatureTel",
                table: "MarriageApplicationForms");

            migrationBuilder.CreateTable(
                name: "Brides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ResidentOf = table.Column<string>(type: "longtext", nullable: false),
                    Genotype = table.Column<string>(type: "longtext", nullable: false),
                    BloodGroup = table.Column<string>(type: "longtext", nullable: false),
                    MaritalStatus = table.Column<string>(type: "longtext", nullable: false),
                    ProposedDowerAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DowerAmountReceivedInCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SignatureTel = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Brides_MarriageApplicationForms_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "MarriageApplicationForms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Token = table.Column<string>(type: "longtext", nullable: false),
                    TargetType = table.Column<int>(type: "int", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: true),
                    MarriageReferenceNumber = table.Column<string>(type: "longtext", nullable: true),
                    RecipientJamaatMemberId = table.Column<Guid>(type: "char(36)", nullable: true),
                    RecipientMembershipNo = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Used = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Brides_MarriageApplicationFormId",
                table: "Brides",
                column: "MarriageApplicationFormId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Brides");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.AddColumn<string>(
                name: "BrideBloodGroup",
                table: "MarriageApplicationForms",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "BrideDateOfBirth",
                table: "MarriageApplicationForms",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "BrideDowerAmountReceivedInCash",
                table: "MarriageApplicationForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BrideGenotype",
                table: "MarriageApplicationForms",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrideMaritalStatus",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrideMembershipNo",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrideName",
                table: "MarriageApplicationForms",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "BrideProposedDowerAmount",
                table: "MarriageApplicationForms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "BrideResidentOf",
                table: "MarriageApplicationForms",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BrideSignatureTel",
                table: "MarriageApplicationForms",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
