using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MustChangePassword = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: true),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Action = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    EntityName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    RecordId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ChangeDetails = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MarriageApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SerialNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarriageApplications", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ClaimType = table.Column<string>(type: "longtext", nullable: true),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "longtext", nullable: true),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Certificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IssuedByUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CertificateFilePath = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_AspNetUsers_IssuedByUserId",
                        column: x => x.IssuedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Certificates_MarriageApplications_MarriageApplicationId",
                        column: x => x.MarriageApplicationId,
                        principalTable: "MarriageApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MarriageApplicationForms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProposedNikahDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Venue = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BrideMembershipNo = table.Column<string>(type: "longtext", nullable: false),
                    BrideName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BrideDateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BrideResidentOf = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    BrideGenotype = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BrideBloodGroup = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BrideMaritalStatus = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    BrideProposedDowerAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrideDowerAmountReceivedInCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrideSignatureTel = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomMembershipNo = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BridegroomDateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BridegroomResidentOf = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    BridegroomGenotype = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BridegroomBloodGroup = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BridegroomDowerAmountPaidInCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BridegroomDowerAmountToBePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFirstNikah = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSecondThirdOrFourthNikah = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormerWifeIsDead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasDivorcedFormerWife = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormerWifeIsPresent = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FormerWifeObtainedKhula = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BridegroomSignatureTel = table.Column<string>(type: "longtext", nullable: false),
                    BrideFatherName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BridegroomFatherName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    GuardianName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    GuardianRelationToBride = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    GuardianAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    GuardianTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    GuardianSignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    RepresentativeName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    RepresentativeAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    RepresentativeActingFor = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RepresentativeSignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    WitnessOneName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    WitnessOneAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    WitnessOneTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    WitnessOneSignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    WitnessTwoName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    WitnessTwoAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    WitnessTwoTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    WitnessTwoSignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    OfficiatingImamName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    OfficiatingImamAddressJamaat = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    OfficiatingImamSignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    JamaatPresidentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    JamaatPresidentSignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    NationalRishtanataSecretaryName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    NationalRishtanataSecretarySignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    ApprovedDateOfNikah = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NationalAmirOrMissionarySignatureDate = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<string>(type: "longtext", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarriageApplicationForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarriageApplicationForms_MarriageApplications_MarriageApplic~",
                        column: x => x.MarriageApplicationId,
                        principalTable: "MarriageApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName_RecordId",
                table: "AuditLogs",
                columns: new[] { "EntityName", "RecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_IssuedByUserId",
                table: "Certificates",
                column: "IssuedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_MarriageApplicationId",
                table: "Certificates",
                column: "MarriageApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarriageApplicationForms_MarriageApplicationId",
                table: "MarriageApplicationForms",
                column: "MarriageApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarriageApplications_SerialNumber",
                table: "MarriageApplications",
                column: "SerialNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "MarriageApplicationForms");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MarriageApplications");
        }
    }
}
