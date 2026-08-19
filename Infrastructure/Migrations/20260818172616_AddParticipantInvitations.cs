using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddParticipantInvitations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParticipantInvitations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false),
                    ParticipantRole = table.Column<int>(type: "int", nullable: false),
                    WitnessOrder = table.Column<int>(type: "int", nullable: true),
                    TokenHash = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "char(36)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParticipantInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParticipantInvitations_FormApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "FormApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantInvitations_ApplicationId",
                table: "ParticipantInvitations",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantInvitations_ExpiresAt",
                table: "ParticipantInvitations",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantInvitations_Status",
                table: "ParticipantInvitations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ParticipantInvitations_TokenHash",
                table: "ParticipantInvitations",
                column: "TokenHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParticipantInvitations");
        }
    }
}
