using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AUT2Services.Infra.Migrations.SqlServer.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutenticadorExterno",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Proveedor = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Usuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ClaveDeAcceso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreADesplegar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValidadorPrimario = table.Column<bool>(type: "bit", nullable: false),
                    AutenticadorExternoBase = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutenticadorExterno_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Entidad",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_UnidadOrganizacional = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Usuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TipoDeEntidad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicioAutorizacion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaTerminoAutorizacion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaCreacion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Principal = table.Column<bool>(type: "bit", nullable: false),
                    EntidadBase = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entidad_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organizacion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdOrganizacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrganizacionBase = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizacion_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoliticaAsignada",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Entidad = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Rol = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Proceso = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FechaInicioAsignacion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaTerminoAsignacion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaCreacion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RolRequiereValidacion = table.Column<bool>(type: "bit", nullable: false),
                    RolAsignadoValidado = table.Column<bool>(type: "bit", nullable: false),
                    PoliticaAsignadaBase = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoliticaAsignada_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proceso",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdMacro_Proceso = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contexto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NivelDeProceso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ComoDesplegarUrlDeProceso = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ProcesoBase = table.Column<bool>(type: "bit", nullable: false),
                    MaximaAsignacionDeRoles = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proceso_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    APIDeAutenticacion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProveedorBase = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id_Rol = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequiereAccionParaSerAsignado = table.Column<bool>(type: "bit", nullable: true),
                    ActivaDetalleDeAutorizaciones = table.Column<bool>(type: "bit", nullable: true),
                    RequiereValidacionDeAsignacion = table.Column<bool>(type: "bit", nullable: true),
                    ValidaAsignacionDeRoles = table.Column<bool>(type: "bit", nullable: true),
                    ValidaEnrrolamiento = table.Column<bool>(type: "bit", nullable: true),
                    RolBase = table.Column<bool>(type: "bit", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true),
                    Nombre = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NombreNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id_Rol);
                });

            migrationBuilder.CreateTable(
                name: "UnidadOrganizacional",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id_Organizacion = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnidadOrganizacionalBase = table.Column<bool>(type: "bit", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnidadOrganizacional_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IdPersona = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NombreADesplegar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoDeUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: true),
                    UsuarioBase = table.Column<bool>(type: "bit", nullable: true),
                    RequiereValidacionEnrrolamiento = table.Column<bool>(type: "bit", nullable: true),
                    EstadoDeUsuario = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InformacionAdicional = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NombreUsuario = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NombreUsuarioNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CorreoElectronicoNormalizado = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CorreoElectronicoConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    HashDeLaClave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroDeTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumeroDeTelefonoConfirmado = table.Column<bool>(type: "bit", nullable: false),
                    DobleFactorHabilitado = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CantidadDeAccesosFallidos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ValidacionEnrrolamiento",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdValidado_Usuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdValidaEnrrolamiento_Usuario = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnrrolamientoAceptado = table.Column<bool>(type: "bit", nullable: false),
                    FechaValidacion = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FechaRegistro = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValidacionEnrrolamiento_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Rol_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Rol",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Rol_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Rol",
                        principalColumn: "Id_Rol",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreatedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RevokedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SelectedOrganization = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    RevocationReason = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Id_Entidad = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Usuario_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AutenticadorExterno",
                columns: new[] { "Id", "Activo", "AutenticadorExternoBase", "ClaveDeAcceso", "Id_Proveedor", "Id_Usuario", "NombreADesplegar", "NombreUsuario", "ValidadorPrimario" },
                values: new object[] { new Guid("ecaf1074-722d-468f-81fa-69c2d7b88d68"), true, true, "AQAAAAIAAYagAAAAEFrF3Wi3ka+83NR4LeucHMod1hOqUy65JTzjnmjTv2e4DtU9+DJiLDTW1IIKhZN7fA==", new Guid("701c19bf-405c-4467-85f0-ddbc3786f9ee"), new Guid("2b12d04f-c167-4ad1-a42a-e2ecd30518d7"), "ADMINISTRADOR", "ADMINISTRADOR", true });

            migrationBuilder.InsertData(
                table: "Entidad",
                columns: new[] { "Id", "CorreoElectronico", "EntidadBase", "FechaCreacion", "FechaInicioAutorizacion", "FechaTerminoAutorizacion", "Id_UnidadOrganizacional", "Id_Usuario", "Principal", "TipoDeEntidad" },
                values: new object[] { new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), "", true, new DateTimeOffset(new DateTime(2026, 4, 6, 19, 42, 37, 511, DateTimeKind.Unspecified).AddTicks(8787), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("198c164d-1cd8-4107-9db3-74b9fa33302c"), new Guid("2b12d04f-c167-4ad1-a42a-e2ecd30518d7"), true, "UNIDAD_ORGANIZACIONAL" });

            migrationBuilder.InsertData(
                table: "Organizacion",
                columns: new[] { "Id", "Activo", "Codigo", "Descripcion", "IdOrganizacion", "Nombre", "OrganizacionBase" },
                values: new object[] { new Guid("70c699b4-eb37-49a4-9dcf-1fc87be16489"), true, "ANID", "Entidad pública encargada de promover la investigación y el desarrollo en Chile.", "", "Agencia Nacional de Investigación y Desarrollo", true });

            migrationBuilder.InsertData(
                table: "PoliticaAsignada",
                columns: new[] { "Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion" },
                values: new object[,]
                {
                    { new Guid("60838d42-c2df-402c-9253-ab3ce52ffb55"), new DateTimeOffset(new DateTime(2026, 4, 6, 23, 42, 37, 513, DateTimeKind.Unspecified).AddTicks(9105), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), new Guid("856a08fd-4162-47cb-bf92-ff25029f3546"), true, true, false },
                    { new Guid("8f589ba2-3bc0-40ea-b7c2-7aaaee278d6d"), new DateTimeOffset(new DateTime(2026, 4, 6, 23, 42, 37, 513, DateTimeKind.Unspecified).AddTicks(7764), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), new Guid("c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e"), true, true, false },
                    { new Guid("bf05f4af-4bbc-472f-a7ed-bbd6d5d1af61"), new DateTimeOffset(new DateTime(2026, 4, 6, 23, 42, 37, 513, DateTimeKind.Unspecified).AddTicks(9101), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), new Guid("03b6b706-a24f-4505-9ef6-e3ae7d48c907"), true, true, false }
                });

            migrationBuilder.InsertData(
                table: "Proceso",
                columns: new[] { "Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url" },
                values: new object[,]
                {
                    { new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), true, "SEGUIMIENTO_FINANCIERO", "IFRAME", "Sistema de Seguimiento Financiero", "Sistema de Seguimiento Financiero", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Seguimiento Financiero", false, "", "http://localhost:4210" },
                    { new Guid("0737dfc9-8c0f-44ed-ada9-0d227d6a4b5c"), true, "SFI_SISTEMA_TERMINO_SIA", "IFRAME", "Sistema Sistema Termino SIA", "Sistema Sistema Termino SIA", new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), 1, "NIVEL_SISTEMA", "Sistema Sistema Termino SIA", false, "", "http://localhost:4210" },
                    { new Guid("233783de-9094-4030-907d-82d7c5abf10e"), true, "POSTULACION", "IFRAME", "Sistema de Postulaciones", "Proceso de Postulacion", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Proceso de Postulacion", false, "", "http://localhost:4210" },
                    { new Guid("2b597b09-55ad-4304-b57d-76bd2df5ac4c"), true, "SFI_SISFON_LUTHIEN_SPI_SCH", "IFRAME", "Sistema Sisfon Luthien SPI SCH", "Sistema Sisfon Luthien SPI SCH", new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), 1, "NIVEL_SISTEMA", "Sistema Sisfon Luthien SPI SCH", false, "", "172.16.4.107:22" },
                    { new Guid("35e9a345-dda9-45fc-9a35-7cf27ec8e947"), true, "VIN_DATACIENCIA", "IFRAME", "Sistema DataCiencia", "Sistema DataCiencia", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema DataCiencia", false, "", "http://localhost:4210" },
                    { new Guid("3f5cceaf-4c86-4495-90b4-aafe7c4ab509"), true, "POS_ACONCAGUA_GENERICO", "REDIRECCION", "Sistema Aconcagua generico", "Sistema Aconcagua Generico", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Sistema Aconcagua Generico", false, "", "http://auth-qa05.anid.cl" },
                    { new Guid("4bb69d17-cc38-4e52-ad36-42226aa5723d"), true, "VIN_BEIC", "IFRAME", "Sistema Beic", "Sistema Beic", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema Beic", false, "", "http://localhost:4210" },
                    { new Guid("587ec39b-e7d8-4c0f-9ac4-697940cf7b07"), true, "VIN_DIODI", "IFRAME", "Sistema DIODI", "Sistema DIODI", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema DIODI", false, "", "http://localhost:4210" },
                    { new Guid("5eeb676d-b3fc-4db7-9421-358a6c26d3dc"), true, "SFO_EVAL_GENERICO", "VENTANA", "Sistema Eval Generico", "Sistema Eval Generico", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema Eval Generico", false, "", "http://localhost:4210" },
                    { new Guid("6bd742e3-0e0a-4990-b51f-661c710ba4b9"), true, "SFO_EVAL_SPI", "VENTANA", "Sistema Eval SPI", "Sistema Eval SPI", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema Eval SPI", false, "", "https://faraondesa.anid.cl/desa2/Evaluacion_TESTING/index.php" },
                    { new Guid("7faa600f-2f47-4168-9a65-3c6d47ef2241"), true, "SFO_EVAL_BECAS", "VENTANA", "Sistema Eval Becas", "Sistema Eval Becas", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema Eval Becas", false, "", "https://servicios-qa.anid.cl/evalbecas" },
                    { new Guid("8ff51db1-44a5-4b9e-b0eb-0cd739cee604"), true, "STE_SISTEMA_VERDE_SIA", "IFRAME", "Sistema Verde SIA", "Sistema Verde SIA", new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), 1, "NIVEL_SISTEMA", "Sistema Verde SIA", false, "", "http://localhost:4210" },
                    { new Guid("9679412f-ae9f-4d77-8629-7559d280ece2"), true, "SFO_FALLO_BECA", "IFRAME", "Sistema Fallo Beca", "Sistema Fallo Beca", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema Fallo Beca", false, "", "https://servicios-qa.anid.cl/web//fallo/#/public" },
                    { new Guid("9c4a10ea-d6f3-4a0c-b4b0-25fb0396c7ee"), true, "VIN_REPOSITORIO", "IFRAME", "Sistema Repositorio", "Sistema Repositorio", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema Repositorio", false, "", "http://localhost:4210" },
                    { new Guid("9cc3ac0d-503a-42b1-bc8e-10d20265e15b"), true, "STE_SISTEMA_TERMINO_SIA", "IFRAME", "Sistema Termino SIA", "Sistema Termino SIA", new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), 1, "NIVEL_SISTEMA", "Sistema Termino SIA", false, "", "http://localhost:4210" },
                    { new Guid("a4ebe253-17c4-4a98-a333-d6fee919a212"), true, "VIN_PDI", "IFRAME", "Sistema PDI", "Sistema PDI", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema PDI", false, "", "http://localhost:4210" },
                    { new Guid("a6151eed-ea81-4edb-9c89-5bf7e260989f"), true, "SFO_FALLO_SPI", "IFRAME", "Sistema Fallo SPI", "Sistema Fallo SPI", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema Fallo SPI", false, "", "http://localhost:4210" },
                    { new Guid("a6ef77f1-e26b-430d-9d94-eef157e4df65"), true, "POS_MILENIO", "REDIRECCION", "Sistema de Postulaciones Milenio", "Proceso de Postulacion Milenio", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Proceso de Postulacion Milenio", false, "", "https://post-im.conicyt.cl/Concursos" },
                    { new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), true, "SEGUIMIENTO_TECNICO", "IFRAME", "Sistema de Seguimiento Técnico", "Sistema de Seguimiento Técnico", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Seguimiento Técnico", false, "", "http://localhost:4210" },
                    { new Guid("afc327c0-3970-40c7-9ef6-c0a3fdb42cb9"), true, "POS_ACONCAGUA_SPI", "REDIRECCION", "Sistema Aconcagua SPI", "Sistema Aconcagua SPI", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Sistema Aconcagua SPI", false, "", "https://auth-qa01.anid.cl" },
                    { new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), true, "SELECCION_FORMALIZACION", "IFRAME", "Sistema de Seleccion y Formalizacion", "Sistema de Seleccion y Formalizacion", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Seleccion y Formalizacion", false, "", "http://localhost:4210" },
                    { new Guid("c6b401ab-5164-43cc-8ae4-28ca2c9edbbe"), true, "POS_GENESIS", "REDIRECCION", "Sistema Genesis", "Sistema Genesis", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Sistema Genesis", false, "", "https://splqa.anid.cl" },
                    { new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), true, "ADMINISTRACION", "IFRAME", "Sistema de Administtracion de Permisos", "Manejo y asignacion de autorizaciones a los usuarios", new Guid("00000000-0000-0000-0000-000000000000"), 0, "NIVEL_MACRO", "Sistema de Autorización", true, "", "http://localhost:4210" },
                    { new Guid("d65b2a0d-0742-491b-be4f-c7c00219821d"), true, "SFI_SYC_FINANCIERO_SIA", "IFRAME", "Sistema SyC Financiero SIA", "Sistema SyC Financiero SIA", new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), 1, "NIVEL_SISTEMA", "Sistema SyC Financiero SIA", false, "", "http://localhost:4210" },
                    { new Guid("e150709d-4fed-480b-8916-8b0837a775d3"), true, "STE_GESTION_MILENIO", "IFRAME", "Sistema Gestion Milenio", "Sistema Gestion Milenio", new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), 1, "NIVEL_SISTEMA", "Sistema Gestion Milenio", false, "", "http://localhost:4210" },
                    { new Guid("e256405c-0bda-479a-8a41-a043c672f9b1"), true, "SFI_SGDL_SPI", "IFRAME", "Sistema SDGL SPI", "Sistema SDGL SPI", new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), 1, "NIVEL_SISTEMA", "Sistema SDGL SPI", false, "", "http://localhost:4210" },
                    { new Guid("eec159f1-9ba9-463d-8407-ec2951751c29"), true, "VIN_SCIELO", "IFRAME", "Sistema Scielo", "Sistema Scielo", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema Scielo", false, "", "http://localhost:4210" },
                    { new Guid("f2224186-ffcc-41ee-bbce-4bbd72504e22"), true, "STE_SIAL_SPI", "IFRAME", "Sistema SIAL SPI", "Sistema SIAL SPI", new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), 1, "NIVEL_SISTEMA", "Sistema SIAL SPI", false, "", "http://localhost:4210" },
                    { new Guid("f3285b94-90a4-4610-8c65-2782d2e3a1a3"), true, "STE_SyC_LEGACY_SIA", "IFRAME", "Sistema SyC Legacy SIA", "Sistema SyC Legacy SIA", new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), 1, "NIVEL_SISTEMA", "Sistema SyC Legacy SIA", false, "", "http://localhost:4210" },
                    { new Guid("f78ccd33-9762-4beb-83bc-7f2b02a295d7"), true, "SFO_FIRMA_CONVENIO", "IFRAME", "Sistema de Seleccion y Autorizacion Firma de Convenio", "Sistema de Seleccion y Autorizacion Firma de Convenio", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema de Seleccion y Autorizacion Firma de Convenio", false, "", "https://servicios-qa.anid.cl/web/firma-convenio/#/login" },
                    { new Guid("fa2e0bda-3063-4357-91b5-17f443b74ebd"), true, "STE_SISFON_LUTHIEN_SPI_SCH", "IFRAME", "Sistema Sisfon Luthien SPI SCH", "Sistema Sisfon Luthien SPI SCH", new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), 1, "NIVEL_SISTEMA", "Sistema Sisfon Luthien SPI SCH", false, "", "172.16.4.107:22" },
                    { new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), true, "VIN_SCIELO", "IFRAME", "Sistema Scielo", "Sistema Scielo", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema Scielo", false, "", "http://localhost:4210" }
                });

            migrationBuilder.InsertData(
                table: "Proveedor",
                columns: new[] { "Id", "APIDeAutenticacion", "Activo", "Codigo", "Descripcion", "Nombre", "ProveedorBase" },
                values: new object[,]
                {
                    { new Guid("701c19bf-405c-4467-85f0-ddbc3786f9ee"), "https://www.aut2.darksidetech.services/", true, "ANID", "Proveedor de autenticación implementadop or ANID.", "ANID", true },
                    { new Guid("9d9b40fa-fecb-41f6-ad4f-8ed7ceb1be13"), "https://api.claveunica.gob.cl", true, "CLAVEUNICA", "Proveedor de autenticación del gobierno chileno para servicios públicos.", "Clave Única", true }
                });

            migrationBuilder.InsertData(
                table: "Rol",
                columns: new[] { "Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento" },
                values: new object[,]
                {
                    { "03b6b706-a24f-4505-9ef6-e3ae7d48c907", false, true, null, "", "VALIDA_ASIGNACION_ROLES", "VALIDA_ASIGNACION_ROLES", true, false, true, false, false },
                    { "36957ec2-2857-4101-a81b-f0340bf8eff2", false, true, null, "", "ADMINISTRADOR_ENTIDAD", "ADMINISTRADOR_ENTIDAD", true, false, true, false, false },
                    { "856a08fd-4162-47cb-bf92-ff25029f3546", false, true, null, "", "VALIDA_ENRROLAMIENTO", "VALIDA_ENRROLAMIENTO", true, false, true, false, false },
                    { "c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e", false, true, null, "", "ADMINISTRADOR", "ADMINISTRADOR", true, false, true, false, false },
                    { "e198ec28-2b1b-48b0-8d5e-eb946d596e90", false, true, null, "", "USUARIO", "USUARIO", false, false, true, false, false },
                    { "e3093727-b36b-45af-a495-7c3d0804c0e9", false, true, null, "", "ADMINISTRADOR_UNIDAD", "ADMINISTRADOR_UNIDAD", true, false, true, false, false }
                });

            migrationBuilder.InsertData(
                table: "UnidadOrganizacional",
                columns: new[] { "Id", "Activo", "Codigo", "Descripcion", "Id_Organizacion", "Nombre", "UnidadOrganizacionalBase" },
                values: new object[] { new Guid("198c164d-1cd8-4107-9db3-74b9fa33302c"), true, "CASA_MATRIZ", "Casa Matriz de la Agencia Nacional de Investigación y Desarrollo", new Guid("70c699b4-eb37-49a4-9dcf-1fc87be16489"), "Casa Matriz ANID", true });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "Id", "CantidadDeAccesosFallidos", "Activo", "ConcurrencyStamp", "Descripcion", "CorreoElectronico", "CorreoElectronicoConfirmado", "EstadoDeUsuario", "IdPersona", "InformacionAdicional", "LockoutEnabled", "LockoutEnd", "NombreADesplegar", "CorreoElectronicoNormalizado", "NombreUsuarioNormalizado", "HashDeLaClave", "NumeroDeTelefono", "NumeroDeTelefonoConfirmado", "RefreshToken", "RefreshTokenExpiresAtUtc", "RequiereValidacionEnrrolamiento", "SecurityStamp", "TipoDeUsuario", "DobleFactorHabilitado", "NombreUsuario", "UsuarioBase" },
                values: new object[] { "2b12d04f-c167-4ad1-a42a-e2ecd30518d7", 10, true, "e5edc734-b9a6-40f6-a23d-0f0df4730666", "Administrador global", "administrador@security.com", true, "REGISTRADO", "", "", false, null, "Administrador", "ADMINISTRADOR@SECURITY.COM", "ADMINISTRADOR", "AQAAAAIAAYagAAAAEFrF3Wi3ka+83NR4LeucHMod1hOqUy65JTzjnmjTv2e4DtU9+DJiLDTW1IIKhZN7fA==", "", true, null, null, false, "", "NACIONAL", false, "ADMINISTRADOR", true });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

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
                name: "IX_RefreshTokens_SessionId",
                table: "RefreshTokens",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_TokenHash",
                table: "RefreshTokens",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Rol",
                column: "NombreNormalizado",
                unique: true,
                filter: "[NombreNormalizado] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Usuario",
                column: "CorreoElectronicoNormalizado");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Usuario",
                column: "NombreUsuarioNormalizado",
                unique: true,
                filter: "[NombreUsuarioNormalizado] IS NOT NULL");
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
                name: "AutenticadorExterno");

            migrationBuilder.DropTable(
                name: "Entidad");

            migrationBuilder.DropTable(
                name: "Organizacion");

            migrationBuilder.DropTable(
                name: "PoliticaAsignada");

            migrationBuilder.DropTable(
                name: "Proceso");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "UnidadOrganizacional");

            migrationBuilder.DropTable(
                name: "ValidacionEnrrolamiento");

            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
