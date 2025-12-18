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
    private const string Id_Proceso_POS_CONVOCATORIA = "afc327c0-3970-40c7-9ef6-c0a3fdb42cb9";
    private const string Id_Proceso_POS_POSTULAR = "3f5cceaf-4c86-4495-90b4-aafe7c4ab509";
    private const string Id_Proceso_POS_PATROCINIO_INSTITUCIONAL = "c6b401ab-5164-43cc-8ae4-28ca2c9edbbe";
    private const string Id_Proceso_POS_CARTAS_DE_RECOMENDACION = "a6ef77f1-e26b-430d-9d94-eef157e4df65";

    private const string Id_Proceso_Seleccion_y_Formalizacion = "c4a10de0-791c-45ec-820c-1a8802cd3e80";
    private const string Id_Proceso_SFO_ADMISIBILIDAD = "6bd742e3-0e0a-4990-b51f-661c710ba4b9";
    private const string Id_Proceso_SFO_EVALUACION = "7faa600f-2f47-4168-9a65-3c6d47ef2241";
    private const string Id_Proceso_SFO_FALLO = "5eeb676d-b3fc-4db7-9421-358a6c26d3dc";
    private const string Id_Proceso_SFO_FIRMA_CONVENIO = "f78ccd33-9762-4beb-83bc-7f2b02a295d7";

    private const string Id_Proceso_SeguimientoFinanciero = "06001a21-9b5f-47a3-ae8b-c749e531f9b1";
    private const string Id_Proceso_SFI_PROYECTOS_PRESUPUESTO = "e256405c-0bda-479a-8a41-a043c672f9b1";
    private const string Id_Proceso_SFI_RENDICIONES = "2b597b09-55ad-4304-b57d-76bd2df5ac4c";

    private const string Id_Proceso_SeguimientoTecnico = "aeaeb19b-2206-4870-9a99-f5d40a982b2e";
    private const string Id_Proceso_STE_PROYECTOS_INFORMES = "f2224186-ffcc-41ee-bbce-4bbd72504e22";

    private const string Id_Proceso_ProductividadCientifica = "fcd2dcf8-7230-4fdd-9651-b4efeb60d11f";
    private const string Id_Proceso_PSC_PORTAL_DEL_INVESTIGADOR = "eec159f1-9ba9-463d-8407-ec2951751c29";
    private const string Id_Proceso_PSC_REPOSITORIO_ANID = "4bb69d17-cc38-4e52-ad36-42226aa5723d";
    private const string Id_Proceso_PSC_DATOS_ABIERTOS = "a4ebe253-17c4-4a98-a333-d6fee919a212";

    private const string Id_Proceso_Expediente = "ed685882-0d73-4fe4-986c-b34f9641622c";
    private const string Id_Proceso_EXP_EXPEDIENTE_ELECTRONICO = "7f618ddf-cadc-4955-b97f-31ebb43cac6f";

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
                    procesoBase: true,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_CONVOCATORIA),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_CONVOCATORIA",
                    nombre: "Proceso de Postulacion Convocatoria",
                    descripcion: "Proceso de Postulacion Convocatoria",
                    contexto: "Sistema de Postulaciones Convocatoria",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_POSTULAR),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_POSTULAR",
                    nombre: "Proceso de Postulacion Postular",
                    descripcion: "Proceso de Postulacion Postular",
                    contexto: "Sistema de Postulaciones Postular",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_PATROCINIO_INSTITUCIONAL),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_PATROCINIO_INSTITUCIONAL",
                    nombre: "Proceso de Postulacion Patrocinio Institucional",
                    descripcion: "Proceso de Postulacion Patrocinio Institucional",
                    contexto: "Sistema de Postulaciones Patrocinio Institucional",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_POS_CARTAS_DE_RECOMENDACION),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Postulacion),
                    codigo: "POS_CARTAS_DE_RECOMENDACION",
                    nombre: "Proceso de Postulacion Cartas de recomendación",
                    descripcion: "Proceso de Postulacion Cartas de recomendación",
                    contexto: "Sistema de Postulaciones Cartas de recomendación",
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
                    id: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "SELECCION_FORMALIZACION",
                    nombre: "Sistema de Seleccion y Autorizacion",
                    descripcion: "Sistema de Seleccion y Autorizacion",
                    contexto: "Sistema de Seleccion y Autorizacion",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: true,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_ADMISIBILIDAD),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_ADMISIBILIDAD",
                    nombre: "Sistema de Seleccion y Autorizacion Admisibilidad",
                    descripcion: "Sistema de Seleccion y Autorizacion Admisibilidad",
                    contexto: "Sistema de Seleccion y Autorizacion Admisibilidad",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_EVALUACION),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_EVALUACION",
                    nombre: "Sistema de Seleccion y Autorizacion Evaluacion",
                    descripcion: "Sistema de Seleccion y Autorizacion Evaluacion",
                    contexto: "Sistema de Seleccion y Autorizacion Evaluacion",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_FALLO),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_FALLO",
                    nombre: "Sistema de Seleccion y Autorizacion Fallo",
                    descripcion: "Sistema de Seleccion y Autorizacion Fallo",
                    contexto: "Sistema de Seleccion y Autorizacion Fallo",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFO_FIRMA_CONVENIO),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Seleccion_y_Formalizacion),
                    codigo: "SFO_FIRMA_CONVENIO",
                    nombre: "Sistema de Seleccion y Autorizacion Firma de Convenio",
                    descripcion: "Sistema de Seleccion y Autorizacion Firma de Convenio",
                    contexto: "Sistema de Seleccion y Autorizacion Firma de Convenio",
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
                    procesoBase: true,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFI_PROYECTOS_PRESUPUESTO),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoFinanciero),
                    codigo: "SFI_PROYECTOS_PRESUPUESTO",
                    nombre: "Sistema de Seguimiento Financiero Proyectos y Presupuesto",
                    descripcion: "Sistema de Seguimiento Financiero Proyectos y Presupuesto",
                    contexto: "Sistema de Seguimiento Financiero Proyectos y Presupuesto",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_SFI_RENDICIONES),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoFinanciero),
                    codigo: "SFI_RENDICIONES",
                    nombre: "Sistema de Seguimiento Financiero Rendiciones",
                    descripcion: "Sistema de Seguimiento Financiero Rendiciones",
                    contexto: "Sistema de Seguimiento Financiero Rendiciones",
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
                    procesoBase: true,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_STE_PROYECTOS_INFORMES),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_SeguimientoTecnico),
                    codigo: "STE_PROYECTOS_INFORMES",
                    nombre: "Sistema de Seguimiento Técnico Proyectos e Informes",
                    descripcion: "Sistema de Seguimiento Técnico Proyectos e Informes",
                    contexto: "Sistema de Seguimiento Técnico Proyectos e Informes",
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
                    id: Guid.Parse(Id_Proceso_ProductividadCientifica),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "PRODUCTIVIDAD_CIENTÍFICA",
                    nombre: "Sistema de Productividad Científica",
                    descripcion: "Sistema de Productividad Científica",
                    contexto: "Sistema de Productividad Científica",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: true,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_PSC_PORTAL_DEL_INVESTIGADOR),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_ProductividadCientifica),
                    codigo: "PSC_PORTAL_DEL_INVESTIGADOR",
                    nombre: "Sistema de Productividad Científica Portal del Investigador",
                    descripcion: "Sistema de Productividad Científica Portal del Investigador",
                    contexto: "Sistema de Productividad Científica Portal del Investigador",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_PSC_REPOSITORIO_ANID),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_ProductividadCientifica),
                    codigo: "PSC_REPOSITORIO_ANID",
                    nombre: "Sistema de Productividad Científica Repositorio ANID",
                    descripcion: "Sistema de Productividad Científica Repositorio ANID",
                    contexto: "Sistema de Productividad Científica Repositorio ANID",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_SISTEMA,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: false,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_PSC_DATOS_ABIERTOS),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_ProductividadCientifica),
                    codigo: "PSC_DATOS_ABIERTOS",
                    nombre: "Sistema de Productividad Científica Datos Abiertos",
                    descripcion: "Sistema de Productividad Científica Datos Abiertos",
                    contexto: "Sistema de Productividad Científica Datos Abiertos",
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
                    id: Guid.Parse(Id_Proceso_Expediente),
                    idMacro_Proceso: Guid.Empty,
                    codigo: "EXPEDIENTE",
                    nombre: "Sistema de Expediente Electrónico",
                    descripcion: "Sistema de Expediente Electrónico",
                    contexto: "Sistema de Expediente Electrónico",
                    nivelDeProceso: EnumNivelDeProceso.NIVEL_MACRO,
                    url: "http://localhost:4210",
                    token: string.Empty,
                    comoDesplegarUrlDeProceso: EnumComoDesplegarUrlDeProceso.IFRAME,
                    procesoBase: true,
                    maximaAsignacionDeRoles: 1,
                    activo: true
                    ),
                new Proceso(
                    id: Guid.Parse(Id_Proceso_EXP_EXPEDIENTE_ELECTRONICO),
                    idMacro_Proceso: Guid.Parse(Id_Proceso_Expediente),
                    codigo: "EXP_EXPEDIENTE_ELECTRONICO",
                    nombre: "Sistema de Expediente Electrónico Expediente",
                    descripcion: "Sistema de Expediente Electrónico Expediente",
                    contexto: "Sistema de Expediente Electrónico Expediente",
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
