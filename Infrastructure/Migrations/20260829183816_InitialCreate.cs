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
                name: "AqeeqahCertificates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    SerialNumber = table.Column<string>(type: "longtext", nullable: false),
                    ChildName = table.Column<string>(type: "longtext", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Gender = table.Column<string>(type: "longtext", nullable: false),
                    PlaceOfBirth = table.Column<string>(type: "longtext", nullable: false),
                    FatherName = table.Column<string>(type: "longtext", nullable: false),
                    MotherName = table.Column<string>(type: "longtext", nullable: false),
                    JamaatId = table.Column<Guid>(type: "char(36)", nullable: false),
                    JamaatName = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    OfficiatingMissionary = table.Column<string>(type: "longtext", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IssuedByUserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    AqeeqahDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AqeeqahLocation = table.Column<string>(type: "longtext", nullable: false),
                    AnimalCount = table.Column<int>(type: "int", nullable: false),
                    CertificateFilePath = table.Column<string>(type: "longtext", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AqeeqahCertificates", x => x.Id);
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
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ChangeDetails = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
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
                name: "FormApplications",
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
                    table.PrimaryKey("PK_FormApplications", x => x.Id);
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
                name: "JamaatRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    UpdatedBy = table.Column<string>(type: "longtext", nullable: false),
                    HierarchyLevel = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JamaatRoles", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Certificates",
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
                    table.PrimaryKey("PK_Certificates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Certificates_FormApplications_MarriageApplicationId",
                        column: x => x.MarriageApplicationId,
                        principalTable: "FormApplications",
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
                    BridegroomFatherName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
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
                    WitnessOneAddress = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false),
                    WitnessOneTel = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    WitnessOneSignatureDate = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    WitnessTwoName = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
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
                    table.PrimaryKey("PK_MarriageApplicationForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarriageApplicationForms_FormApplications_MarriageApplicatio~",
                        column: x => x.MarriageApplicationId,
                        principalTable: "FormApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                    MaidenName = table.Column<string>(type: "longtext", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhoneNo = table.Column<string>(type: "longtext", nullable: true),
                    JamaatName = table.Column<string>(type: "longtext", nullable: false),
                    CircuitName = table.Column<string>(type: "longtext", nullable: false),
                    Sex = table.Column<string>(type: "longtext", nullable: false),
                    MaritalStatus = table.Column<string>(type: "longtext", nullable: true),
                    Address = table.Column<string>(type: "longtext", nullable: true),
                    NextOfKinPhoneNo = table.Column<string>(type: "longtext", nullable: true),
                    NextOfKinName = table.Column<string>(type: "longtext", nullable: true),
                    NextOfKinAddress = table.Column<string>(type: "longtext", nullable: true),
                    Nationality = table.Column<string>(type: "longtext", nullable: true),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsSystemDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    NewRole = table.Column<string>(type: "longtext", nullable: false),
                    ResetToken = table.Column<string>(type: "longtext", nullable: true),
                    ResetTokenExpiry = table.Column<DateTime>(type: "datetime(6)", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_JamaatMembers_JamaatRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "JamaatRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                name: "BrideGrooms",
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
                    ReferenceNumber = table.Column<string>(type: "longtext", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrideGrooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BrideGrooms_MarriageApplicationForms_MarriageApplicationForm~",
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
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Address = table.Column<string>(type: "longtext", nullable: false),
                    Tel = table.Column<string>(type: "longtext", nullable: false),
                    SignatureDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Signature = table.Column<string>(type: "longtext", nullable: true),
                    WitnessContext = table.Column<int>(type: "int", nullable: false),
                    WitnessNumber = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_AmirApprovalSection_MarriageApplicationFormId",
                table: "AmirApprovalSection",
                column: "MarriageApplicationFormId",
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
                name: "IX_BrideFormSection_MarriageApplicationFormId",
                table: "BrideFormSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrideGrooms_MarriageApplicationFormId",
                table: "BrideGrooms",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_MarriageApplicationId",
                table: "Certificates",
                column: "MarriageApplicationId",
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
                name: "IX_JamaatMembers_BrideGuardianId",
                table: "JamaatMembers",
                column: "BrideGuardianId");

            migrationBuilder.CreateIndex(
                name: "IX_JamaatMembers_RoleId",
                table: "JamaatMembers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_JamaatPresidentVerificationSection_MarriageApplicationFormId",
                table: "JamaatPresidentVerificationSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarriageApplicationForms_MarriageApplicationId",
                table: "MarriageApplicationForms",
                column: "MarriageApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MarriageFormRejections_MarriageApplicationFormId",
                table: "MarriageFormRejections",
                column: "MarriageApplicationFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_MarriageApplicationId",
                table: "Reviews",
                column: "MarriageApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RishtanataRecommendationSection_MarriageApplicationFormId",
                table: "RishtanataRecommendationSection",
                column: "MarriageApplicationFormId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WitnessSignatureSection_MarriageApplicationFormId",
                table: "WitnessSignatureSection",
                column: "MarriageApplicationFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AmirApprovalSection");

            migrationBuilder.DropTable(
                name: "AqeeqahCertificates");

            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "BrideFormSection");

            migrationBuilder.DropTable(
                name: "BrideGrooms");

            migrationBuilder.DropTable(
                name: "Certificates");

            migrationBuilder.DropTable(
                name: "GuardianOrWakeelSection");

            migrationBuilder.DropTable(
                name: "ImamVerificationSection");

            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "JamaatPresidentVerificationSection");

            migrationBuilder.DropTable(
                name: "MarriageFormRejections");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "RishtanataRecommendationSection");

            migrationBuilder.DropTable(
                name: "WitnessSignatureSection");

            migrationBuilder.DropTable(
                name: "JamaatMembers");

            migrationBuilder.DropTable(
                name: "MarriageApplicationForms");

            migrationBuilder.DropTable(
                name: "BrideGuardian");

            migrationBuilder.DropTable(
                name: "JamaatRoles");

            migrationBuilder.DropTable(
                name: "FormApplications");
        }
    }
}
