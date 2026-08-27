using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBrideGroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BrideGrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    BridegroomMembershipNo = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomName = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomDateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BridegroomResidentOf = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomGenotype = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomBloodGroup = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomDowerAmountPaidInCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BridegroomDowerAmountToBePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFirstNikah = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSecondThirdOrFourthNikah = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormerWifeIsDead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasDivorcedFormerWife = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormerWifeIsPresent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormerWifeObtainedKhula = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BridegroomSignatureTel = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrideGrooms", x => x.Id);
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrideGrooms");

            migrationBuilder.DropTable(
                name: "Invitations");
        }
    }
}
