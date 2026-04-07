using AUT2Services.Domain.Entities;
using AUT2Services.Domain.Enumerations;
using AUT2Services.Domain.Security.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AUT2Services.Infra.Data.Extensions;

public static class IncludeBaseData
{
    private const string Id_Administrador = "2b12d04f-c167-4ad1-a42a-e2ecd30518d7";

    private const string Id_Rol_Administrador = "c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e";
    private const string Id_Rol_Administrador_Entidad = "36957ec2-2857-4101-a81b-f0340bf8eff2";
    private const string Id_Rol_Administrador_Unidad = "e3093727-b36b-45af-a495-7c3d0804c0e9";
    private const string Id_Rol_Valida_Asignacion_Roles = "03b6b706-a24f-4505-9ef6-e3ae7d48c907";
    private const string Id_Rol_Valida_Enrrolamiento = "856a08fd-4162-47cb-bf92-ff25029f3546";
    private const string Id_Rol_Usuario = "e198ec28-2b1b-48b0-8d5e-eb946d596e90";

    private const string Id_Organizacion_ANID = "70c699b4-eb37-49a4-9dcf-1fc87be16489";

    private const string Id_UnidadOrganizacional_ANID_CasaMatriz = "198c164d-1cd8-4107-9db3-74b9fa33302c";

    private const string Id_Proceso_Administracion = "d1889c7c-c5dc-4d9a-a2fe-34cdf956b145";

    private const string Id_Proceso_Postulacion = "233783de-9094-4030-907d-82d7c5abf10e";
    private const string Id_Proceso_POS_Aconcagua_API = "afc327c0-3970-40c7-9ef6-c0a3fdb42cb9";
    private const string Id_Proceso_POS_Aconcagua_Generico = "3f5cceaf-4c86-4495-90b4-aafe7c4ab509";
    private const string Id_Proceso_POS_Genesis = "c6b401ab-5164-43cc-8ae4-28ca2c9edbbe";
    private const string Id_Proceso_POS_Milenio = "a6ef77f1-e26b-430d-9d94-eef157e4df65";

    private const string Id_Proceso_Seleccion_y_Formalizacion = "c4a10de0-791c-45ec-820c-1a8802cd3e80";
    private const string Id_Proceso_SFO_Eval_SPI = "6bd742e3-0e0a-4990-b51f-661c710ba4b9";
    private const string Id_Proceso_SFO_Eval_Becas = "7faa600f-2f47-4168-9a65-3c6d47ef2241";
    private const string Id_Proceso_SFO_Eval_Generico = "5eeb676d-b3fc-4db7-9421-358a6c26d3dc";
    private const string Id_Proceso_SFO_Firma_Convenio = "f78ccd33-9762-4beb-83bc-7f2b02a295d7";
    private const string Id_Proceso_SFO_Fallo_Beca = "9679412f-ae9f-4d77-8629-7559d280ece2";
    private const string Id_Proceso_SFO_Fallo_SPI = "a6151eed-ea81-4edb-9c89-5bf7e260989f";

    private const string Id_Proceso_SeguimientoTecnico = "aeaeb19b-2206-4870-9a99-f5d40a982b2e";
    private const string Id_Proceso_STE_SIAL_SPI = "f2224186-ffcc-41ee-bbce-4bbd72504e22";
    private const string Id_Proceso_STE_SyC_Legacy_SIA = "f3285b94-90a4-4610-8c65-2782d2e3a1a3";
    private const string Id_Proceso_STE_sisfon_luthien_SPI_SCH = "fa2e0bda-3063-4357-91b5-17f443b74ebd";
    private const string Id_Proceso_STE_Sistema_Verde_SIA = "8ff51db1-44a5-4b9e-b0eb-0cd739cee604";
    private const string Id_Proceso_STE_Gestion_Milenio = "e150709d-4fed-480b-8916-8b0837a775d3";
    private const string Id_Proceso_STE_Sistema_Termino_SIA = "9cc3ac0d-503a-42b1-bc8e-10d20265e15b";

    private const string Id_Proceso_SeguimientoFinanciero = "06001a21-9b5f-47a3-ae8b-c749e531f9b1";
    private const string Id_Proceso_SFI_SGDL_SPI = "e256405c-0bda-479a-8a41-a043c672f9b1";
    private const string Id_Proceso_SFI_Sisfon_Luthien_SPI_SCH = "2b597b09-55ad-4304-b57d-76bd2df5ac4c";
    private const string Id_Proceso_SFI_SyC_Financiero_SIA = "d65b2a0d-0742-491b-be4f-c7c00219821d";
    private const string Id_Proceso_SFI_Sistema_Termino_SIA = "0737dfc9-8c0f-44ed-ada9-0d227d6a4b5c";

    private const string Id_Proceso_Vinculacion = "fcd2dcf8-7230-4fdd-9651-b4efeb60d11f";
    private const string Id_Proceso_VIN_Scielo = "eec159f1-9ba9-463d-8407-ec2951751c29";
    private const string Id_Proceso_VIN_Beic = "4bb69d17-cc38-4e52-ad36-42226aa5723d";
    private const string Id_Proceso_VIN_PDI = "a4ebe253-17c4-4a98-a333-d6fee919a212";
    private const string Id_Proceso_VIN_DataCiencia = "35e9a345-dda9-45fc-9a35-7cf27ec8e947";
    private const string Id_Proceso_VIN_Repositorio = "9c4a10ea-d6f3-4a0c-b4b0-25fb0396c7ee";
    private const string Id_Proceso_VIN_DIODI = "587ec39b-e7d8-4c0f-9ac4-697940cf7b07";

    private const string Id_Entidad_Administrador = "05507441-5792-4c46-9334-9a5faa99e20a";

    private const string Id_PoliticaAsignada_1 = "8f589ba2-3bc0-40ea-b7c2-7aaaee278d6d";
    private const string Id_PoliticaAsignada_2 = "bf05f4af-4bbc-472f-a7ed-bbd6d5d1af61";
    private const string Id_PoliticaAsignada_3 = "60838d42-c2df-402c-9253-ab3ce52ffb55";

    private const string Id_Proveedor_ClaveUnica = "9d9b40fa-fecb-41f6-ad4f-8ed7ceb1be13";
    private const string Id_Proveedor_ANID = "701c19bf-405c-4467-85f0-ddbc3786f9ee";

    private const string Id_AutenticadorExterno_ADMINISTRADOR = "ecaf1074-722d-468f-81fa-69c2d7b88d68";

    private static string clave_Administrador = string.Empty;

    public static void SeedBaseData(ModelBuilder modelBuilder)
    {
        SeedDataSecurity(modelBuilder);

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SessionId).HasMaxLength(64).IsRequired();
            entity.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            entity.Property(x => x.SelectedOrganization).HasMaxLength(256);
            entity.Property(x => x.ReplacedByTokenHash).HasMaxLength(128);
            entity.Property(x => x.RevocationReason).HasMaxLength(64);
            entity.Property(x => x.UserId).IsRequired();
            entity.Property(x => x.Id_Entidad).IsRequired();
            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.HasIndex(x => x.SessionId);

            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

    public static void SeedDataSecurity(ModelBuilder modelBuilder)
    {
        SeedRoles(modelBuilder);
        SeedUsers(modelBuilder);
        SeedOrganizaciones(modelBuilder);
        SeedUnidadesOrganizacionales(modelBuilder);
        SeedProcesos(modelBuilder);
        SeedEntidades(modelBuilder);
        SeedPoliticasAsignadas(modelBuilder);
        SeedProveedores(modelBuilder);
        SeedAutenticadorExterno(modelBuilder);
    }

    public static void ConfigureBaseDataSecurity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.SessionId).HasMaxLength(64).IsRequired();
            entity.Property(x => x.TokenHash).HasMaxLength(128).IsRequired();
            entity.Property(x => x.ReplacedByTokenHash).HasMaxLength(128);
            entity.Property(x => x.RevocationReason).HasMaxLength(64);
            entity.Property(x => x.UserId).IsRequired();
            entity.HasIndex(x => x.TokenHash).IsUnique();
            entity.HasIndex(x => x.SessionId);

            entity.HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });


        modelBuilder.Entity<Rol>(entity =>
        {
            entity.ToTable(name: "Rol");

            entity.Property(p => p.Id).HasColumnName("Id_Rol");
            entity.Property(p => p.Name).HasColumnName("Nombre");
            entity.Property(p => p.NormalizedName).HasColumnName("NombreNormalizado");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable(name: "Usuario");

            entity.Property(p => p.UserName).HasColumnName("NombreUsuario");
            entity.Property(p => p.NormalizedUserName).HasColumnName("NombreUsuarioNormalizado");
            entity.Property(p => p.Email).HasColumnName("CorreoElectronico");
            entity.Property(p => p.NormalizedEmail).HasColumnName("CorreoElectronicoNormalizado");
            entity.Property(p => p.EmailConfirmed).HasColumnName("CorreoElectronicoConfirmado");
            entity.Property(p => p.PasswordHash).HasColumnName("HashDeLaClave");
            entity.Property(p => p.PhoneNumber).HasColumnName("NumeroDeTelefono");
            entity.Property(p => p.PhoneNumberConfirmed).HasColumnName("NumeroDeTelefonoConfirmado");
            entity.Property(p => p.TwoFactorEnabled).HasColumnName("DobleFactorHabilitado");
            entity.Property(p => p.AccessFailedCount).HasColumnName("CantidadDeAccesosFallidos");
        });
    }

    private static void SeedUsers(ModelBuilder modelBuilder)
    {
        var hasher = new PasswordHasher<Usuario>();

        var usuarioAdministrador = new Usuario
        {
            Id = Id_Administrador,
            UserName = EnumUsuariosBase.ADMINISTRADOR,
            NormalizedUserName = EnumUsuariosBase.ADMINISTRADOR,
            Email = "administrador@security.com",
            NormalizedEmail = "administrador@security.com".ToUpper(),
            EmailConfirmed = true,
            SecurityStamp = string.Empty,
            PhoneNumber = string.Empty,
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            AccessFailedCount = 10,
            #region PropiedadesPersonalizadas
            IdPersona = string.Empty,
            NombreADesplegar = "Administrador",
            Descripcion = "Administrador global",
            TipoDeUsuario = EnumTipoDeUsuario.NACIONAL,
            Activo = true,
            UsuarioBase = true,
            RequiereValidacionEnrrolamiento = false,
            EstadoDeUsuario = EnumEstadoDeUsuario.REGISTRADO,
            InformacionAdicional = string.Empty,
            #endregion PropiedadesPersonalizadas
        };

        usuarioAdministrador.PasswordHash = hasher.HashPassword(usuarioAdministrador, "Changeme123#");
        clave_Administrador = usuarioAdministrador.PasswordHash;

        modelBuilder.Entity<Usuario>().HasData(
            usuarioAdministrador
            );
    }

    private static void SeedRoles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rol>()
            .HasData(
                new Rol
                {
                    Id = Id_Rol_Administrador,
                    Name = EnumRolesBase.ADMINISTRADOR,
                    NormalizedName = EnumRolesBase.ADMINISTRADOR,
                    #region PropiedadesPersonalizadas
                    ActivaDetalleDeAutorizaciones = false,
                    RequiereAccionParaSerAsignado = true,
                    ValidaAsignacionDeRoles = false,
                    ValidaEnrrolamiento = false,
                    RolBase = true,
                    Activo = true
                    #endregion PropiedadesPersonalizadas
                },
                new Rol
                {
                    Id = Id_Rol_Administrador_Entidad,
                    Name = EnumRolesBase.ADMINISTRADOR_ENTIDAD,
                    NormalizedName = EnumRolesBase.ADMINISTRADOR_ENTIDAD,
                    #region PropiedadesPersonalizadas
                    ActivaDetalleDeAutorizaciones = false,
                    RequiereAccionParaSerAsignado = true,
                    ValidaAsignacionDeRoles = false,
                    ValidaEnrrolamiento = false,
                    RolBase = true,
                    Activo = true
                    #endregion PropiedadesPersonalizadas
                },
                new Rol
                {
                    Id = Id_Rol_Administrador_Unidad,
                    Name = EnumRolesBase.ADMINISTRADOR_UNIDAD,
                    NormalizedName = EnumRolesBase.ADMINISTRADOR_UNIDAD,
                    #region PropiedadesPersonalizadas
                    ActivaDetalleDeAutorizaciones = false,
                    RequiereAccionParaSerAsignado = true,
                    ValidaAsignacionDeRoles = false,
                    ValidaEnrrolamiento = false,
                    RolBase = true,
                    Activo = true
                    #endregion PropiedadesPersonalizadas
                },
                new Rol
                {
                    Id = Id_Rol_Valida_Asignacion_Roles,
                    Name = EnumRolesBase.VALIDA_ASIGNACION_ROLES,
                    NormalizedName = EnumRolesBase.VALIDA_ASIGNACION_ROLES,
                    #region PropiedadesPersonalizadas
                    ActivaDetalleDeAutorizaciones = false,
                    RequiereAccionParaSerAsignado = true,
                    ValidaAsignacionDeRoles = false,
                    ValidaEnrrolamiento = false,
                    RolBase = true,
                    Activo = true
                    #endregion PropiedadesPersonalizadas
                },
                new Rol
                {
                    Id = Id_Rol_Valida_Enrrolamiento,
                    Name = EnumRolesBase.VALIDA_ENRROLAMIENTO,
                    NormalizedName = EnumRolesBase.VALIDA_ENRROLAMIENTO,
                    #region PropiedadesPersonalizadas
                    ActivaDetalleDeAutorizaciones = false,
                    RequiereAccionParaSerAsignado = true,
                    ValidaAsignacionDeRoles = false,
                    ValidaEnrrolamiento = false,
                    RolBase = true,
                    Activo = true
                    #endregion PropiedadesPersonalizadas
                },
                new Rol
                {
                    Id = Id_Rol_Usuario,
                    Name = EnumRolesBase.USUARIO,
                    NormalizedName = EnumRolesBase.USUARIO,
                    #region PropiedadesPersonalizadas
                    ActivaDetalleDeAutorizaciones = false,
                    RequiereAccionParaSerAsignado = false,
                    ValidaAsignacionDeRoles = false,
                    ValidaEnrrolamiento = false,
                    RolBase = true,
                    Activo = true
                    #endregion PropiedadesPersonalizadas
                }
            );
    }

    private static void SeedOrganizaciones(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Organizacion>()
            .HasData(
                new Organizacion(
                    id: Guid.Parse(Id_Organizacion_ANID),
                    idOrganizacion: "",
                    codigo: EnumOrganizacionBase.ANID,
                    nombre: "Agencia Nacional de Investigación y Desarrollo",
                    descripcion: "Entidad pública encargada de promover la investigación y el desarrollo en Chile.",
                    organizacionBase: true,
                    activo: true
                    )
            );
    }

    private static void SeedUnidadesOrganizacionales(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UnidadOrganizacional>()
            .HasData(
                new UnidadOrganizacional(
                    id: Guid.Parse(Id_UnidadOrganizacional_ANID_CasaMatriz),
                    id_Organizacion: Guid.Parse(Id_Organizacion_ANID),
                    codigo: EnumUnidadOrganizacionalBase.CASA_MATRIZ,
                    nombre: "Casa Matriz ANID",
                    descripcion: "Casa Matriz de la Agencia Nacional de Investigación y Desarrollo",
                    unidadOrganizacionalBase: true,
                    activo: true
                    )
            );
    }

    private static void SeedProcesos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Proceso>()
            .HasData(
                new Proceso(
                    id: Guid.Parse(Id_Proceso_Administracion),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "ADMINISTRACION",
                    nombre: "Sistema de Autorización",
                    descripcion: "Manejo y asignacion de autorizaciones a los usuarios",
                    contexto: "Sistema de Administtracion de Permisos",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: true,
                    maximaAsignacionDeRoles: 0,
                    activo: true
                    )
            );

        modelBuilder.Entity<Proceso>()
            .HasData(
                new Proceso(
                    id: Guid.Parse(Id_Proceso_Postulacion),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "POSTULACION",
                    nombre: "Proceso de Postulacion",
                    descripcion: "Proceso de Postulacion",
                    contexto: "Sistema de Postulaciones",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_Aconcagua_API),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_ACONCAGUA_SPI",
                    nombre: "Sistema Aconcagua SPI",
                    descripcion: "Sistema Aconcagua SPI",
                    contexto: "Sistema Aconcagua SPI",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "https://auth-qa01.anid.cl",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.REDIRECCION,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_Aconcagua_Generico),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_ACONCAGUA_GENERICO",
                    nombre: "Sistema Aconcagua Generico",
                    descripcion: "Sistema Aconcagua Generico",
                    contexto: "Sistema Aconcagua generico",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://auth-qa05.anid.cl",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.REDIRECCION,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_Genesis),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_GENESIS",
                    nombre: "Sistema Genesis",
                    descripcion: "Sistema Genesis",
                    contexto: "Sistema Genesis",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "https://splqa.anid.cl",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.REDIRECCION,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_Milenio),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_MILENIO",
                    nombre: "Proceso de Postulacion Milenio",
                    descripcion: "Proceso de Postulacion Milenio",
                    contexto: "Sistema de Postulaciones Milenio",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "https://post-im.conicyt.cl/Concursos",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.REDIRECCION,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    )
            );

        modelBuilder.Entity<Proceso>()
            .HasData(
                new Proceso(
                    id: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "SELECCION_FORMALIZACION",
                    nombre: "Sistema de Seleccion y Formalizacion",
                    descripcion: "Sistema de Seleccion y Formalizacion",
                    contexto: "Sistema de Seleccion y Formalizacion",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_Eval_SPI),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_EVAL_SPI",
                    nombre: "Sistema Eval SPI",
                    descripcion: "Sistema Eval SPI",
                    contexto: "Sistema Eval SPI",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "https://faraondesa.anid.cl/desa2/Evaluacion_TESTING/index.php",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.VENTANA,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_Eval_Becas),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_EVAL_BECAS",
                    nombre: "Sistema Eval Becas",
                    descripcion: "Sistema Eval Becas",
                    contexto: "Sistema Eval Becas",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "https://servicios-qa.anid.cl/evalbecas",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.VENTANA,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_Eval_Generico),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_EVAL_GENERICO",
                    nombre: "Sistema Eval Generico",
                    descripcion: "Sistema Eval Generico",
                    contexto: "Sistema Eval Generico",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.VENTANA,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_Firma_Convenio),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_FIRMA_CONVENIO",
                    nombre: "Sistema de Seleccion y Autorizacion Firma de Convenio",
                    descripcion: "Sistema de Seleccion y Autorizacion Firma de Convenio",
                    contexto: "Sistema de Seleccion y Autorizacion Firma de Convenio",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "https://servicios-qa.anid.cl/web/firma-convenio/#/login",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_Fallo_Beca),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_FALLO_BECA",
                    nombre: "Sistema Fallo Beca",
                    descripcion: "Sistema Fallo Beca",
                    contexto: "Sistema Fallo Beca",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "https://servicios-qa.anid.cl/web//fallo/#/public",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_Fallo_SPI),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_FALLO_SPI",
                    nombre: "Sistema Fallo SPI",
                    descripcion: "Sistema Fallo SPI",
                    contexto: "Sistema Fallo SPI",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    )
            );

        modelBuilder.Entity<Proceso>()
            .HasData(
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "SEGUIMIENTO_TECNICO",
                    nombre: "Sistema de Seguimiento Técnico",
                    descripcion: "Sistema de Seguimiento Técnico",
                    contexto: "Sistema de Seguimiento Técnico",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_STE_SIAL_SPI),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    codigo: "STE_SIAL_SPI",
                    nombre: "Sistema SIAL SPI",
                    descripcion: "Sistema SIAL SPI",
                    contexto: "Sistema SIAL SPI",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_STE_SyC_Legacy_SIA),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    codigo: "STE_SyC_LEGACY_SIA",
                    nombre: "Sistema SyC Legacy SIA",
                    descripcion: "Sistema SyC Legacy SIA",
                    contexto: "Sistema SyC Legacy SIA",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_STE_sisfon_luthien_SPI_SCH),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    codigo: "STE_SISFON_LUTHIEN_SPI_SCH",
                    nombre: "Sistema Sisfon Luthien SPI SCH",
                    descripcion: "Sistema Sisfon Luthien SPI SCH",
                    contexto: "Sistema Sisfon Luthien SPI SCH",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "172.16.4.107:22",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_STE_Sistema_Verde_SIA),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    codigo: "STE_SISTEMA_VERDE_SIA",
                    nombre: "Sistema Verde SIA",
                    descripcion: "Sistema Verde SIA",
                    contexto: "Sistema Verde SIA",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_STE_Gestion_Milenio),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    codigo: "STE_GESTION_MILENIO",
                    nombre: "Sistema Gestion Milenio",
                    descripcion: "Sistema Gestion Milenio",
                    contexto: "Sistema Gestion Milenio",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_STE_Sistema_Termino_SIA),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    codigo: "STE_SISTEMA_TERMINO_SIA",
                    nombre: "Sistema Termino SIA",
                    descripcion: "Sistema Termino SIA",
                    contexto: "Sistema Termino SIA",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    )
            );

        modelBuilder.Entity<Proceso>()
            .HasData(
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SeguimientoFinanciero),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "SEGUIMIENTO_FINANCIERO",
                    nombre: "Sistema de Seguimiento Financiero",
                    descripcion: "Sistema de Seguimiento Financiero",
                    contexto: "Sistema de Seguimiento Financiero",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFI_SGDL_SPI),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoFinanciero),
                    codigo: "SFI_SGDL_SPI",
                    nombre: "Sistema SDGL SPI",
                    descripcion: "Sistema SDGL SPI",
                    contexto: "Sistema SDGL SPI",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFI_Sisfon_Luthien_SPI_SCH),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoFinanciero),
                    codigo: "SFI_SISFON_LUTHIEN_SPI_SCH",
                    nombre: "Sistema Sisfon Luthien SPI SCH",
                    descripcion: "Sistema Sisfon Luthien SPI SCH",
                    contexto: "Sistema Sisfon Luthien SPI SCH",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "172.16.4.107:22",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFI_SyC_Financiero_SIA),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoFinanciero),
                    codigo: "SFI_SYC_FINANCIERO_SIA",
                    nombre: "Sistema SyC Financiero SIA",
                    descripcion: "Sistema SyC Financiero SIA",
                    contexto: "Sistema SyC Financiero SIA",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFI_Sistema_Termino_SIA),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoFinanciero),
                    codigo: "SFI_SISTEMA_TERMINO_SIA",
                    nombre: "Sistema Sistema Termino SIA",
                    descripcion: "Sistema Sistema Termino SIA",
                    contexto: "Sistema Sistema Termino SIA",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    )
            );

        modelBuilder.Entity<Proceso>()
            .HasData(
                new Proceso(
                    id: Guid.Parse(Id_Proceso_Vinculacion),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "VIN_SCIELO",
                    nombre: "Sistema Scielo",
                    descripcion: "Sistema Scielo",
                    contexto: "Sistema Scielo",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_VIN_Scielo),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Vinculacion),
                    codigo: "VIN_SCIELO",
                    nombre: "Sistema Scielo",
                    descripcion: "Sistema Scielo",
                    contexto: "Sistema Scielo",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_VIN_Beic),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Vinculacion),
                    codigo: "VIN_BEIC",
                    nombre: "Sistema Beic",
                    descripcion: "Sistema Beic",
                    contexto: "Sistema Beic",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_VIN_PDI),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Vinculacion),
                    codigo: "VIN_PDI",
                    nombre: "Sistema PDI",
                    descripcion: "Sistema PDI",
                    contexto: "Sistema PDI",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_VIN_DataCiencia),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Vinculacion),
                    codigo: "VIN_DATACIENCIA",
                    nombre: "Sistema DataCiencia",
                    descripcion: "Sistema DataCiencia",
                    contexto: "Sistema DataCiencia",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_VIN_Repositorio),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Vinculacion),
                    codigo: "VIN_REPOSITORIO",
                    nombre: "Sistema Repositorio",
                    descripcion: "Sistema Repositorio",
                    contexto: "Sistema Repositorio",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_VIN_DIODI),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Vinculacion),
                    codigo: "VIN_DIODI",
                    nombre: "Sistema DIODI",
                    descripcion: "Sistema DIODI",
                    contexto: "Sistema DIODI",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    )
            );
    }

    private static void SeedEntidades(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entidad>()
            .HasData(
                new Entidad(
                    id: Guid.Parse(Id_Entidad_Administrador),
                    id_UnidadOrganizacional: Guid.Parse(Id_UnidadOrganizacional_ANID_CasaMatriz),
                    id_Usuario: Guid.Parse(Id_Administrador),
                    tipoDeEntidad: EnumTipoDeEntidad.UNIDAD_ORGANIZACIONAL,
                    correoElectronico: "",
                    fechaInicioAutorizacion: DateTimeOffset.MinValue,
                    fechaTerminoAutorizacion: DateTimeOffset.MaxValue,
                    fechaCreacion: DateTimeOffset.Now,
                    principal: true,
                    entidadBase: true
                    )
            );
    }

    private static void SeedPoliticasAsignadas(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PoliticaAsignada>()
            .HasData(
                new PoliticaAsignada(
                    id: Guid.Parse(Id_PoliticaAsignada_1),
                    id_Entidad: Guid.Parse(Id_Entidad_Administrador),
                    id_Rol: Guid.Parse(Id_Rol_Administrador),
                    id_Proceso: Guid.Parse(Id_Proceso_Administracion),
                    fechaInicioAsignacion: DateTimeOffset.MinValue,
                    fechaTerminoAsignacion: DateTimeOffset.MaxValue,
                    fechaCreacion: DateTimeOffset.UtcNow,
                    rolRequiereValidacion: false,
                    rolAsignadoValidado: true,
                    politicaAsignadaBase: true
                    ),
                new PoliticaAsignada(
                    id: Guid.Parse(Id_PoliticaAsignada_2),
                    id_Entidad: Guid.Parse(Id_Entidad_Administrador),
                    id_Rol: Guid.Parse(Id_Rol_Valida_Asignacion_Roles),
                    id_Proceso: Guid.Parse(Id_Proceso_Administracion),
                    fechaInicioAsignacion: DateTimeOffset.MinValue,
                    fechaTerminoAsignacion: DateTimeOffset.MaxValue,
                    fechaCreacion: DateTimeOffset.UtcNow,
                    rolRequiereValidacion: false,
                    rolAsignadoValidado: true,
                    politicaAsignadaBase: true
                    ),
                new PoliticaAsignada(
                    id: Guid.Parse(Id_PoliticaAsignada_3),
                    id_Entidad: Guid.Parse(Id_Entidad_Administrador),
                    id_Rol: Guid.Parse(Id_Rol_Valida_Enrrolamiento),
                    id_Proceso: Guid.Parse(Id_Proceso_Administracion),
                    fechaInicioAsignacion: DateTimeOffset.MinValue,
                    fechaTerminoAsignacion: DateTimeOffset.MaxValue,
                    fechaCreacion: DateTimeOffset.UtcNow,
                    rolRequiereValidacion: false,
                    rolAsignadoValidado: true,
                    politicaAsignadaBase: true
                    )
            );
    }

    private static void SeedProveedores(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Proveedor>()
            .HasData(
                new Proveedor(
                    id: Guid.Parse(Id_Proveedor_ClaveUnica),
                    codigo: "CLAVEUNICA",
                    nombre: "Clave Única",
                    descripcion: "Proveedor de autenticación del gobierno chileno para servicios públicos.",
                    aPIDeAutenticacion: "https://api.claveunica.gob.cl",
                    proveedorBase: true,
                    activo: true
                    ),
                new Proveedor(
                    id: Guid.Parse(Id_Proveedor_ANID),
                    codigo: "ANID",
                    nombre: "ANID",
                    descripcion: "Proveedor de autenticación implementadop or ANID.",
                    aPIDeAutenticacion: "https://www.aut2.darksidetech.services/",
                    proveedorBase: true,
                    activo: true
                    )
            );
    }

    private static void SeedAutenticadorExterno(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AutenticadorExterno>()
            .HasData(
                new AutenticadorExterno(
                    id: Guid.Parse(Id_AutenticadorExterno_ADMINISTRADOR),
                    id_Proveedor: Guid.Parse(Id_Proveedor_ANID),
                    id_Usuario: Guid.Parse(Id_Administrador),
                    nombreUsuario: EnumUsuariosBase.ADMINISTRADOR,
                    claveDeAcceso: clave_Administrador,
                    nombreADesplegar: EnumUsuariosBase.ADMINISTRADOR,
                    validadorPrimario: true,
                    autenticadorExternoBase: true,
                    activo: true
                    )
            );
    }
}
