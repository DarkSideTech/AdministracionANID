using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUT2Services.Infra.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditTraceability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditAggregateCursor",
                columns: table => new
                {
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastRevision = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditAggregateCursor", x => x.AggregateId);
                });

            migrationBuilder.CreateTable(
                name: "AuditOutbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CorrelationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AggregateType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AggregateRevision = table.Column<long>(type: "bigint", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CommandType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    ActorUserId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ActorUsername = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ActorEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    RequestPath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    OccurredAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    PersistedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SnapshotJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DispatchStatus = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    DispatchAttempts = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastDispatchAttemptUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DispatchedAtUtc = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastError = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemaVersion = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditOutbox", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditOutboxChange",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuditOutboxMessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ValueType = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    NewValueJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditOutboxChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditOutboxChange_AuditOutbox_AuditOutboxMessageId",
                        column: x => x.AuditOutboxMessageId,
                        principalTable: "AuditOutbox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutbox_ActorUserId_OccurredAtUtc",
                table: "AuditOutbox",
                columns: new[] { "ActorUserId", "OccurredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutbox_AggregateId_AggregateRevision",
                table: "AuditOutbox",
                columns: new[] { "AggregateId", "AggregateRevision" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutbox_AggregateId_OccurredAtUtc_Id",
                table: "AuditOutbox",
                columns: new[] { "AggregateId", "OccurredAtUtc", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutbox_CorrelationId",
                table: "AuditOutbox",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutbox_DispatchStatus_PersistedAtUtc_Id",
                table: "AuditOutbox",
                columns: new[] { "DispatchStatus", "PersistedAtUtc", "Id" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutbox_EventType_OccurredAtUtc",
                table: "AuditOutbox",
                columns: new[] { "EventType", "OccurredAtUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutboxChange_AuditOutboxMessageId",
                table: "AuditOutboxChange",
                column: "AuditOutboxMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditOutboxChange_Path",
                table: "AuditOutboxChange",
                column: "Path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditAggregateCursor");

            migrationBuilder.DropTable(
                name: "AuditOutboxChange");

            migrationBuilder.DropTable(
                name: "AuditOutbox");
        }
    }
}
