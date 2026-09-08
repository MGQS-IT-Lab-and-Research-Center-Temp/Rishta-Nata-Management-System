using System;
using Microsoft.EntityFrameworkCore.Migrations;

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
                name: "ApplicationSubmissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Status = table.Column<string>(type: "longtext", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CertificateId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationSubmissions", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    EntityName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RecordId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ChangeDetails = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BrideGuardian",
                columns: table => new
                {
                    BrideGuardianId = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    GuardianName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    GuardianRelationToBride = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    GuardianAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    GuardianTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    GuardianSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
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

            migrationBuilder.CreateTable(
                name: "NikahApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApplicationStage = table.Column<int>(type: "int", nullable: true),
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ProposedNikahDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Venue = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BrideMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    BrideName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BrideDateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BrideResidentOf = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    BrideGenotype = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BrideBloodGroup = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BrideMaritalStatus = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    BrideProposedDowerAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrideDowerAmountReceivedInCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrideSignatureTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    BridegroomMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
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
                    BridegroomSignatureTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    BrideFatherName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BrideFatherMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    BridegroomFatherName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BridegroomFatherMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    GuardianName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    GuardianRelationToBride = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    GuardianAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    GuardianTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    GuardianSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RepresentativeName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    RepresentativeAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    RepresentativeActingFor = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RepresentativeSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    WitnessOneName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    WitnessOneMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    WitnessOneAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    WitnessOneTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    WitnessOneSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    WitnessTwoName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    WitnessTwoMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    WitnessTwoAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    WitnessTwoTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    WitnessTwoSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    OfficiatingImamName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    OfficiatingImamAddressJamaat = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    OfficiatingImamSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    JamaatPresidentName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    JamaatPresidentSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    NationalRishtanataSecretaryName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    NationalRishtanataSecretarySignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ApprovedDateOfNikah = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NationalAmirOrMissionarySignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    FormStage = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NikahApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NikahApplications_ApplicationSubmissions_MarriageApplication~",
                        column: x => x.MarriageApplicationId,
                        principalTable: "ApplicationSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NikahCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SerialNumber = table.Column<string>(type: "longtext", nullable: false),
                    BrideName = table.Column<string>(type: "longtext", nullable: false),
                    BrideFatherName = table.Column<string>(type: "longtext", nullable: false),
                    BrideResidentOf = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomName = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomFatherName = table.Column<string>(type: "longtext", nullable: false),
                    BridegroomResidentOf = table.Column<string>(type: "longtext", nullable: false),
                    NikahDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DowryAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MarriageApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IssuedByUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CertificateFilePath = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NikahCertificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NikahCertificates_ApplicationSubmissions_MarriageApplication~",
                        column: x => x.MarriageApplicationId,
                        principalTable: "ApplicationSubmissions",
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
                        name: "FK_Reviews_ApplicationSubmissions_MarriageApplicationId",
                        column: x => x.MarriageApplicationId,
                        principalTable: "ApplicationSubmissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JamaatMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Surname = table.Column<string>(type: "longtext", nullable: false),
                    FirstName = table.Column<string>(type: "longtext", nullable: false),
                    Email = table.Column<string>(type: "longtext", nullable: false),
                    ChandaNo = table.Column<string>(type: "longtext", nullable: false),
                    WasiyatNo = table.Column<string>(type: "longtext", nullable: true),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    AuxillaryBodyName = table.Column<string>(type: "longtext", nullable: true),
                    MiddleName = table.Column<string>(type: "longtext", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneNo = table.Column<string>(type: "longtext", nullable: true),
                    JamaatName = table.Column<string>(type: "longtext", nullable: false),
                    CircuitName = table.Column<string>(type: "longtext", nullable: false),
                    Sex = table.Column<string>(type: "longtext", nullable: false),
                    MaritalStatus = table.Column<string>(type: "longtext", nullable: true),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    Nationality = table.Column<string>(type: "longtext", nullable: true),
                    Roles = table.Column<string>(type: "longtext", nullable: false),
                    BrideGuardianId = table.Column<Guid>(type: "char(36)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JamaatMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JamaatMembers_BrideGuardian_BrideGuardianId",
                        column: x => x.BrideGuardianId,
                        principalTable: "BrideGuardian",
                        principalColumn: "BrideGuardianId",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AmirApprovals",
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
                    table.PrimaryKey("PK_AmirApprovals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmirApprovals_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ImamVerifications",
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
                    table.PrimaryKey("PK_ImamVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImamVerifications_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "JamaatPresidentVerifications",
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
                    table.PrimaryKey("PK_JamaatPresidentVerifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JamaatPresidentVerifications_NikahApplications_MarriageAppli~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NikahBrides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    BrideMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    BrideName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    BrideDateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BrideResidentOf = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    BrideGenotype = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BrideBloodGroup = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    BrideMaritalStatus = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    BrideProposedDowerAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrideDowerAmountReceivedInCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BrideSignatureTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NikahBrides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NikahBrides_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NikahGrooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
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
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NikahGrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NikahGrooms_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NikahRejections",
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
                    table.PrimaryKey("PK_NikahRejections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NikahRejections_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RishtanataRecommendations",
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
                    table.PrimaryKey("PK_RishtanataRecommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RishtanataRecommendations_NikahApplications_MarriageApplicat~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SectionAccessTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    SectionType = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    CreatedByMembershipNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SectionAccessTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SectionAccessTokens_NikahApplications_MarriageApplicationFor~",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WitnessSignatures",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    Tel = table.Column<string>(type: "longtext", nullable: false),
                    SignatureDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Signature = table.Column<string>(type: "longtext", nullable: true),
                    WitnessContext = table.Column<int>(type: "int", nullable: false),
                    WitnessNumber = table.Column<int>(type: "int", nullable: false),
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    MarriageApplicationFormId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WitnessSignatures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WitnessSignatures_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NikahGuardians",
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
                    ReferenceNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    JamaatMemberId = table.Column<Guid>(type: "char(36)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NikahGuardians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NikahGuardians_JamaatMembers_JamaatMemberId",
                        column: x => x.JamaatMemberId,
                        principalTable: "JamaatMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NikahGuardians_NikahApplications_MarriageApplicationFormId",
                        column: x => x.MarriageApplicationFormId,
                        principalTable: "NikahApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AmirApprovals_MarriageApplicationFormId",
                table: "AmirApprovals",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_CreatedAt",
                table: "AuditLogs",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityName_RecordId",
                table: "AuditLogs",
                columns: new[] { "EntityName", "RecordId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ImamVerifications_MarriageApplicationFormId",
                table: "ImamVerifications",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JamaatMembers_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_JamaatPresidentVerifications_MarriageApplicationFormId",
                table: "JamaatPresidentVerifications",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NikahApplications_MarriageApplicationId",
                table: "NikahApplications",
                column: "MarriageApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NikahBrides_MarriageApplicationFormId",
                table: "NikahBrides",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NikahCertificates_MarriageApplicationId",
                table: "NikahCertificates",
                column: "MarriageApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NikahGrooms_MarriageApplicationFormId",
                table: "NikahGrooms",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NikahGuardians_JamaatMemberId",
                table: "NikahGuardians",
                column: "JamaatMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_NikahGuardians_MarriageApplicationFormId",
                table: "NikahGuardians",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NikahRejections_MarriageApplicationFormId",
                table: "NikahRejections",
                column: "MarriageApplicationFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MarriageApplicationId",
                table: "Reviews",
                column: "MarriageApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RishtanataRecommendations_MarriageApplicationFormId",
                table: "RishtanataRecommendations",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SectionAccessTokens_MarriageApplicationFormId_SectionType",
                table: "SectionAccessTokens",
                columns: new[] { "MarriageApplicationFormId", "SectionType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WitnessSignatures_MarriageApplicationFormId",
                table: "WitnessSignatures",
                column: "MarriageApplicationFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmirApprovals");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "ImamVerifications");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "JamaatPresidentVerifications");

            migrationBuilder.DropTable(
                name: "NikahBrides");

            migrationBuilder.DropTable(
                name: "NikahCertificates");

            migrationBuilder.DropTable(
                name: "NikahGrooms");

            migrationBuilder.DropTable(
                name: "NikahGuardians");

            migrationBuilder.DropTable(
                name: "NikahRejections");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "RishtanataRecommendations");

            migrationBuilder.DropTable(
                name: "SectionAccessTokens");

            migrationBuilder.DropTable(
                name: "WitnessSignatures");

            migrationBuilder.DropTable(
                name: "JamaatMembers");

            migrationBuilder.DropTable(
                name: "NikahApplications");

            migrationBuilder.DropTable(
                name: "BrideGuardian");

            migrationBuilder.DropTable(
                name: "ApplicationSubmissions");
        }
    }
}
