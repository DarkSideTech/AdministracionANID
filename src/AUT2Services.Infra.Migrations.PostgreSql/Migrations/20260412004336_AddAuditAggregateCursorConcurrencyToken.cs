using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AUT2Services.Infra.Migrations.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditAggregateCursorConcurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ConcurrencyToken",
                table: "AuditAggregateCursor",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.InsertData(
                table: "PoliticaAsignada",
                columns: new[] { "Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion" },
                values: new object[] { new Guid("60838d42-c2df-402c-9253-ab3ce52ffb62"), new DateTimeOffset(new DateTime(2026, 4, 7, 23, 4, 30, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), new Guid("e198ec28-2b1b-48b0-8d5e-eb946d596e91"), true, true, false });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento" },
                values: new object[] { "e198ec28-2b1b-48b0-8d5e-eb946d596e91", false, true, "4f4d7775-2677-47e7-8ccd-a34f5d002007", "", "AUDITOR_TRAZABILIDAD", "AUDITOR_TRAZABILIDAD", true, false, true, false, false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PoliticaAsignada",
                keyColumn: "Id",
                keyValue: new Guid("60838d42-c2df-402c-9253-ab3ce52ffb62"));

            migrationBuilder.DeleteData(
                table: "Rol",
                keyColumn: "Id_Rol",
                keyValue: "e198ec28-2b1b-48b0-8d5e-eb946d596e91");

            migrationBuilder.DropColumn(
                name: "ConcurrencyToken",
                table: "AuditAggregateCursor");
        }
    }
}
