using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJamaatPresidentInformationRequiredStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Certificates_AspNetUsers_IssuedByUserId",
                table: "Certificates");

            migrationBuilder.DropIndex(
                name: "IX_Certificates_IssuedByUserId",
                table: "Certificates");

            migrationBuilder.DropColumn(
                name: "IssuedByUserId",
                table: "Certificates");

            migrationBuilder.AlterColumn<string>(
                name: "WitnessTwoSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "WitnessOneSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "RepresentativeSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "OfficiatingImamSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "NationalRishtanataSecretarySignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "NationalAmirOrMissionarySignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "JamaatPresidentSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "GuardianSignatureDate",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "BridegroomSignatureTel",
                table: "MarriageApplicationForms",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "BridegroomMembershipNo",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "BrideSignatureTel",
                table: "MarriageApplicationForms",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "BrideMembershipNo",
                table: "MarriageApplicationForms",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WitnessTwoSignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "WitnessOneSignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RepresentativeSignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "OfficiatingImamSignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NationalRishtanataSecretarySignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NationalAmirOrMissionarySignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "JamaatPresidentSignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "GuardianSignatureDate",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "BridegroomSignatureTel",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "BridegroomMembershipNo",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "BrideSignatureTel",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "BrideMembershipNo",
                table: "MarriageApplicationForms",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "IssuedByUserId",
                table: "Certificates",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Certificates_IssuedByUserId",
                table: "Certificates",
                column: "IssuedByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Certificates_AspNetUsers_IssuedByUserId",
                table: "Certificates",
                column: "IssuedByUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
