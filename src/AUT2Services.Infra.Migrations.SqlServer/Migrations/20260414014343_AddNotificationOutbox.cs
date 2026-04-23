using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUT2Services.Infra.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationOutbox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationOutbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Channel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RequestPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DeduplicationKey = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PayloadJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DispatchStatus = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    DispatchAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    CreatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    NextAttemptUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastDispatchAttemptUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DispatchedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemaVersion = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationOutbox", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOutbox_Channel_NotificationType_CreatedAtUtc",
                table: "NotificationOutbox",
                columns: new[] { "Channel", "NotificationType", "CreatedAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOutbox_DeduplicationKey",
                table: "NotificationOutbox",
                column: "DeduplicationKey");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOutbox_DispatchStatus_NextAttemptUtc_CreatedAtUtc_Id",
                table: "NotificationOutbox",
                columns: new[] { "DispatchStatus", "NextAttemptUtc", "CreatedAtUtc", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationOutbox_UserId",
                table: "NotificationOutbox",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationOutbox");
        }
    }
}
