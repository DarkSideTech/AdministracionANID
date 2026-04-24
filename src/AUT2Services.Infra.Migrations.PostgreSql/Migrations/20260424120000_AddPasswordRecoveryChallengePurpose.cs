using AUT2Services.Infra.Data.Context;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUT2Services.Infra.Migrations.PostgreSql.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(AUT2ServicesContext))]
    [Migration("20260424120000_AddPasswordRecoveryChallengePurpose")]
    public partial class AddPasswordRecoveryChallengePurpose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PasswordChangeChallenges_UserId_ExpiresAtUtc_ConsumedAtUtc_~",
                table: "PasswordChangeChallenges");

            migrationBuilder.AddColumn<string>(
                name: "ChallengePurpose",
                table: "PasswordChangeChallenges",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "PASSWORD_CHANGE");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordChangeChallenges_UserId_Purpose_Expires",
                table: "PasswordChangeChallenges",
                columns: new[] { "UserId", "ChallengePurpose", "ExpiresAtUtc", "ConsumedAtUtc", "CancelledAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PasswordChangeChallenges_UserId_Purpose_Expires",
                table: "PasswordChangeChallenges");

            migrationBuilder.DropColumn(
                name: "ChallengePurpose",
                table: "PasswordChangeChallenges");

            migrationBuilder.CreateIndex(
                name: "IX_PasswordChangeChallenges_UserId_ExpiresAtUtc_ConsumedAtUtc_~",
                table: "PasswordChangeChallenges",
                columns: new[] { "UserId", "ExpiresAtUtc", "ConsumedAtUtc", "CancelledAtUtc" });
        }
    }
}
