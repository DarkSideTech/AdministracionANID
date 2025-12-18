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

            migrationBuilder.InsertData(
                table: "AutenticadorExterno",
                columns: new[] { "Id", "Activo", "AutenticadorExternoBase", "ClaveDeAcceso", "Id_Proveedor", "Id_Usuario", "NombreADesplegar", "NombreUsuario", "ValidadorPrimario" },
                values: new object[] { new Guid("ecaf1074-722d-468f-81fa-69c2d7b88d68"), true, true, "AQAAAAIAAYagAAAAEJ4PR5McQ5LU8RXggNqnBrS3qdIO54mYO8+1rkT1vSryd4FBlMVPKuLbopBp0XUJKw==", new Guid("701c19bf-405c-4467-85f0-ddbc3786f9ee"), new Guid("2b12d04f-c167-4ad1-a42a-e2ecd30518d7"), "ADMINISTRADOR", "ADMINISTRADOR", true });

            migrationBuilder.InsertData(
                table: "Entidad",
                columns: new[] { "Id", "CorreoElectronico", "EntidadBase", "FechaCreacion", "FechaInicioAutorizacion", "FechaTerminoAutorizacion", "Id_UnidadOrganizacional", "Id_Usuario", "Principal", "TipoDeEntidad" },
                values: new object[] { new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), "", true, new DateTimeOffset(new DateTime(2025, 12, 3, 11, 36, 56, 571, DateTimeKind.Unspecified).AddTicks(1214), new TimeSpan(0, -3, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("198c164d-1cd8-4107-9db3-74b9fa33302c"), new Guid("2b12d04f-c167-4ad1-a42a-e2ecd30518d7"), true, "UNIDAD_ORGANIZACIONAL" });

            migrationBuilder.InsertData(
                table: "Organizacion",
                columns: new[] { "Id", "Activo", "Codigo", "Descripcion", "IdOrganizacion", "Nombre", "OrganizacionBase" },
                values: new object[] { new Guid("70c699b4-eb37-49a4-9dcf-1fc87be16489"), true, "ANID", "Entidad pública encargada de promover la investigación y el desarrollo en Chile.", "", "Agencia Nacional de Investigación y Desarrollo", true });

            migrationBuilder.InsertData(
                table: "PoliticaAsignada",
                columns: new[] { "Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion" },
                values: new object[,]
                {
                    { new Guid("60838d42-c2df-402c-9253-ab3ce52ffb55"), new DateTimeOffset(new DateTime(2025, 12, 3, 14, 36, 56, 571, DateTimeKind.Unspecified).AddTicks(5033), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), new Guid("856a08fd-4162-47cb-bf92-ff25029f3546"), true, true, false },
                    { new Guid("8f589ba2-3bc0-40ea-b7c2-7aaaee278d6d"), new DateTimeOffset(new DateTime(2025, 12, 3, 14, 36, 56, 571, DateTimeKind.Unspecified).AddTicks(3775), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), new Guid("c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e"), true, true, false },
                    { new Guid("bf05f4af-4bbc-472f-a7ed-bbd6d5d1af61"), new DateTimeOffset(new DateTime(2025, 12, 3, 14, 36, 56, 571, DateTimeKind.Unspecified).AddTicks(5027), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), new Guid("05507441-5792-4c46-9334-9a5faa99e20a"), new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), new Guid("03b6b706-a24f-4505-9ef6-e3ae7d48c907"), true, true, false }
                });

            migrationBuilder.InsertData(
                table: "Proceso",
                columns: new[] { "Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url" },
                values: new object[,]
                {
                    { new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), true, "SEGUIMIENTO_FINANCIERO", "IFRAME", "Sistema de Seguimiento Financiero", "Sistema de Seguimiento Financiero", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Seguimiento Financiero", true, "", "http://localhost:4210" },
                    { new Guid("233783de-9094-4030-907d-82d7c5abf10e"), true, "POSTULACION", "IFRAME", "Sistema de Postulaciones", "Proceso de Postulacion", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Proceso de Postulacion", true, "", "http://localhost:4210" },
                    { new Guid("2b597b09-55ad-4304-b57d-76bd2df5ac4c"), true, "SFI_RENDICIONES", "IFRAME", "Sistema de Seguimiento Financiero Rendiciones", "Sistema de Seguimiento Financiero Rendiciones", new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), 1, "NIVEL_SISTEMA", "Sistema de Seguimiento Financiero Rendiciones", false, "", "http://localhost:4210" },
                    { new Guid("3f5cceaf-4c86-4495-90b4-aafe7c4ab509"), true, "POS_POSTULAR", "IFRAME", "Sistema de Postulaciones Postular", "Proceso de Postulacion Postular", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Proceso de Postulacion Postular", false, "", "http://localhost:4210" },
                    { new Guid("4bb69d17-cc38-4e52-ad36-42226aa5723d"), true, "PSC_REPOSITORIO_ANID", "IFRAME", "Sistema de Productividad Científica Repositorio ANID", "Sistema de Productividad Científica Repositorio ANID", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema de Productividad Científica Repositorio ANID", false, "", "http://localhost:4210" },
                    { new Guid("5eeb676d-b3fc-4db7-9421-358a6c26d3dc"), true, "SFO_FALLO", "IFRAME", "Sistema de Seleccion y Autorizacion Fallo", "Sistema de Seleccion y Autorizacion Fallo", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema de Seleccion y Autorizacion Fallo", false, "", "http://localhost:4210" },
                    { new Guid("6bd742e3-0e0a-4990-b51f-661c710ba4b9"), true, "SFO_ADMISIBILIDAD", "IFRAME", "Sistema de Seleccion y Autorizacion Admisibilidad", "Sistema de Seleccion y Autorizacion Admisibilidad", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema de Seleccion y Autorizacion Admisibilidad", false, "", "http://localhost:4210" },
                    { new Guid("7f618ddf-cadc-4955-b97f-31ebb43cac6f"), true, "EXP_EXPEDIENTE_ELECTRONICO", "IFRAME", "Sistema de Expediente Electrónico Expediente", "Sistema de Expediente Electrónico Expediente", new Guid("ed685882-0d73-4fe4-986c-b34f9641622c"), 1, "NIVEL_SISTEMA", "Sistema de Expediente Electrónico Expediente", false, "", "http://localhost:4210" },
                    { new Guid("7faa600f-2f47-4168-9a65-3c6d47ef2241"), true, "SFO_EVALUACION", "IFRAME", "Sistema de Seleccion y Autorizacion Evaluacion", "Sistema de Seleccion y Autorizacion Evaluacion", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema de Seleccion y Autorizacion Evaluacion", false, "", "http://localhost:4210" },
                    { new Guid("a4ebe253-17c4-4a98-a333-d6fee919a212"), true, "PSC_DATOS_ABIERTOS", "IFRAME", "Sistema de Productividad Científica Datos Abiertos", "Sistema de Productividad Científica Datos Abiertos", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema de Productividad Científica Datos Abiertos", false, "", "http://localhost:4210" },
                    { new Guid("a6ef77f1-e26b-430d-9d94-eef157e4df65"), true, "POS_CARTAS_DE_RECOMENDACION", "IFRAME", "Sistema de Postulaciones Cartas de recomendación", "Proceso de Postulacion Cartas de recomendación", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Proceso de Postulacion Cartas de recomendación", false, "", "http://localhost:4210" },
                    { new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), true, "SEGUIMIENTO_TECNICO", "IFRAME", "Sistema de Seguimiento Técnico", "Sistema de Seguimiento Técnico", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Seguimiento Técnico", true, "", "http://localhost:4210" },
                    { new Guid("afc327c0-3970-40c7-9ef6-c0a3fdb42cb9"), true, "POS_CONVOCATORIA", "IFRAME", "Sistema de Postulaciones Convocatoria", "Proceso de Postulacion Convocatoria", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Proceso de Postulacion Convocatoria", false, "", "http://localhost:4210" },
                    { new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), true, "SELECCION_FORMALIZACION", "IFRAME", "Sistema de Seleccion y Autorizacion", "Sistema de Seleccion y Autorizacion", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Seleccion y Autorizacion", true, "", "http://localhost:4210" },
                    { new Guid("c6b401ab-5164-43cc-8ae4-28ca2c9edbbe"), true, "POS_PATROCINIO_INSTITUCIONAL", "IFRAME", "Sistema de Postulaciones Patrocinio Institucional", "Proceso de Postulacion Patrocinio Institucional", new Guid("233783de-9094-4030-907d-82d7c5abf10e"), 1, "NIVEL_SISTEMA", "Proceso de Postulacion Patrocinio Institucional", false, "", "http://localhost:4210" },
                    { new Guid("d1889c7c-c5dc-4d9a-a2fe-34cdf956b145"), true, "ADMINISTRACION", "IFRAME", "Sistema de Administtracion de Permisos", "Manejo y asignacion de autorizaciones a los usuarios", new Guid("00000000-0000-0000-0000-000000000000"), 0, "NIVEL_MACRO", "Sistema de Autorización", true, "", "http://localhost:4210" },
                    { new Guid("e256405c-0bda-479a-8a41-a043c672f9b1"), true, "SFI_PROYECTOS_PRESUPUESTO", "IFRAME", "Sistema de Seguimiento Financiero Proyectos y Presupuesto", "Sistema de Seguimiento Financiero Proyectos y Presupuesto", new Guid("06001a21-9b5f-47a3-ae8b-c749e531f9b1"), 1, "NIVEL_SISTEMA", "Sistema de Seguimiento Financiero Proyectos y Presupuesto", false, "", "http://localhost:4210" },
                    { new Guid("ed685882-0d73-4fe4-986c-b34f9641622c"), true, "EXPEDIENTE", "IFRAME", "Sistema de Expediente Electrónico", "Sistema de Expediente Electrónico", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Expediente Electrónico", true, "", "http://localhost:4210" },
                    { new Guid("eec159f1-9ba9-463d-8407-ec2951751c29"), true, "PSC_PORTAL_DEL_INVESTIGADOR", "IFRAME", "Sistema de Productividad Científica Portal del Investigador", "Sistema de Productividad Científica Portal del Investigador", new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), 1, "NIVEL_SISTEMA", "Sistema de Productividad Científica Portal del Investigador", false, "", "http://localhost:4210" },
                    { new Guid("f2224186-ffcc-41ee-bbce-4bbd72504e22"), true, "STE_PROYECTOS_INFORMES", "IFRAME", "Sistema de Seguimiento Técnico Proyectos e Informes", "Sistema de Seguimiento Técnico Proyectos e Informes", new Guid("aeaeb19b-2206-4870-9a99-f5d40a982b2e"), 1, "NIVEL_SISTEMA", "Sistema de Seguimiento Técnico Proyectos e Informes", false, "", "http://localhost:4210" },
                    { new Guid("f78ccd33-9762-4beb-83bc-7f2b02a295d7"), true, "SFO_FIRMA_CONVENIO", "IFRAME", "Sistema de Seleccion y Autorizacion Firma de Convenio", "Sistema de Seleccion y Autorizacion Firma de Convenio", new Guid("c4a10de0-791c-45ec-820c-1a8802cd3e80"), 1, "NIVEL_SISTEMA", "Sistema de Seleccion y Autorizacion Firma de Convenio", false, "", "http://localhost:4210" },
                    { new Guid("fcd2dcf8-7230-4fdd-9651-b4efeb60d11f"), true, "PRODUCTIVIDAD_CIENTÍFICA", "IFRAME", "Sistema de Productividad Científica", "Sistema de Productividad Científica", new Guid("00000000-0000-0000-0000-000000000000"), 1, "NIVEL_MACRO", "Sistema de Productividad Científica", true, "", "http://localhost:4210" }
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
                values: new object[] { "2b12d04f-c167-4ad1-a42a-e2ecd30518d7", 10, true, "72e725ba-e2c5-42d3-8fb9-4b7949c73c47", "Administrador global", "administrador@security.com", true, "REGISTRADO", "", "", false, null, "Administrador", "ADMINISTRADOR@SECURITY.COM", "ADMINISTRADOR", "AQAAAAIAAYagAAAAEJ4PR5McQ5LU8RXggNqnBrS3qdIO54mYO8+1rkT1vSryd4FBlMVPKuLbopBp0XUJKw==", "", true, null, null, false, "", "NACIONAL", false, "ADMINISTRADOR", true });

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
