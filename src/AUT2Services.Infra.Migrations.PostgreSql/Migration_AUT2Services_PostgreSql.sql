CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "AutenticadorExterno" (
        "Id" uuid NOT NULL,
        "Id_Proveedor" uuid NOT NULL,
        "Id_Usuario" uuid NOT NULL,
        "NombreUsuario" character varying(255) NOT NULL,
        "ClaveDeAcceso" text NOT NULL,
        "NombreADesplegar" text NOT NULL,
        "ValidadorPrimario" boolean NOT NULL,
        "AutenticadorExternoBase" boolean NOT NULL,
        "Activo" boolean NOT NULL,
        CONSTRAINT "PK_AutenticadorExterno_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "Entidad" (
        "Id" uuid NOT NULL,
        "Id_UnidadOrganizacional" uuid NOT NULL,
        "Id_Usuario" uuid NOT NULL,
        "TipoDeEntidad" character varying(100) NOT NULL,
        "CorreoElectronico" text NOT NULL,
        "FechaInicioAutorizacion" timestamp with time zone NOT NULL,
        "FechaTerminoAutorizacion" timestamp with time zone NOT NULL,
        "FechaCreacion" timestamp with time zone NOT NULL,
        "Principal" boolean NOT NULL,
        "EntidadBase" boolean NOT NULL,
        CONSTRAINT "PK_Entidad_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "Organizacion" (
        "Id" uuid NOT NULL,
        "IdOrganizacion" text NOT NULL,
        "Codigo" character varying(100) NOT NULL,
        "Nombre" character varying(255) NOT NULL,
        "Descripcion" text NOT NULL,
        "OrganizacionBase" boolean NOT NULL,
        "Activo" boolean NOT NULL,
        CONSTRAINT "PK_Organizacion_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "PoliticaAsignada" (
        "Id" uuid NOT NULL,
        "Id_Entidad" uuid NOT NULL,
        "Id_Rol" uuid NOT NULL,
        "Id_Proceso" uuid NOT NULL,
        "FechaInicioAsignacion" timestamp with time zone NOT NULL,
        "FechaTerminoAsignacion" timestamp with time zone NOT NULL,
        "FechaCreacion" timestamp with time zone NOT NULL,
        "RolRequiereValidacion" boolean NOT NULL,
        "RolAsignadoValidado" boolean NOT NULL,
        "PoliticaAsignadaBase" boolean NOT NULL,
        CONSTRAINT "PK_PoliticaAsignada_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "Proceso" (
        "Id" uuid NOT NULL,
        "IdMacro_Proceso" uuid NOT NULL,
        "Codigo" character varying(100) NOT NULL,
        "Nombre" character varying(255) NOT NULL,
        "Descripcion" text NOT NULL,
        "Contexto" text NOT NULL,
        "NivelDeProceso" character varying(100) NOT NULL,
        "Url" text NOT NULL,
        "Token" text NOT NULL,
        "ComoDesplegarUrlDeProceso" character varying(100) NOT NULL,
        "ProcesoBase" boolean NOT NULL,
        "MaximaAsignacionDeRoles" integer NOT NULL,
        "Activo" boolean NOT NULL,
        CONSTRAINT "PK_Proceso_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "Proveedor" (
        "Id" uuid NOT NULL,
        "Codigo" character varying(100) NOT NULL,
        "Nombre" character varying(255) NOT NULL,
        "Descripcion" character varying(500) NOT NULL,
        "APIDeAutenticacion" text NOT NULL,
        "ProveedorBase" boolean NOT NULL,
        "Activo" boolean NOT NULL,
        CONSTRAINT "PK_Proveedor_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "Rol" (
        "Id_Rol" text NOT NULL,
        "Descripcion" text,
        "RequiereAccionParaSerAsignado" boolean,
        "ActivaDetalleDeAutorizaciones" boolean,
        "RequiereValidacionDeAsignacion" boolean,
        "ValidaAsignacionDeRoles" boolean,
        "ValidaEnrrolamiento" boolean,
        "RolBase" boolean,
        "Activo" boolean,
        "Nombre" character varying(256),
        "NombreNormalizado" character varying(256),
        "ConcurrencyStamp" text,
        CONSTRAINT "PK_Rol" PRIMARY KEY ("Id_Rol")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "UnidadOrganizacional" (
        "Id" uuid NOT NULL,
        "Id_Organizacion" uuid NOT NULL,
        "Codigo" character varying(100) NOT NULL,
        "Nombre" character varying(255) NOT NULL,
        "Descripcion" text NOT NULL,
        "UnidadOrganizacionalBase" boolean NOT NULL,
        "Activo" boolean NOT NULL,
        CONSTRAINT "PK_UnidadOrganizacional_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "Usuario" (
        "Id" text NOT NULL,
        "IdPersona" text,
        "NombreADesplegar" text,
        "Descripcion" text,
        "TipoDeUsuario" text,
        "Activo" boolean,
        "UsuarioBase" boolean,
        "RequiereValidacionEnrrolamiento" boolean,
        "EstadoDeUsuario" text,
        "InformacionAdicional" text,
        "RefreshToken" text,
        "RefreshTokenExpiresAtUtc" timestamp with time zone,
        "NombreUsuario" character varying(256),
        "NombreUsuarioNormalizado" character varying(256),
        "CorreoElectronico" character varying(256),
        "CorreoElectronicoNormalizado" character varying(256),
        "CorreoElectronicoConfirmado" boolean NOT NULL,
        "HashDeLaClave" text,
        "SecurityStamp" text,
        "ConcurrencyStamp" text,
        "NumeroDeTelefono" text,
        "NumeroDeTelefonoConfirmado" boolean NOT NULL,
        "DobleFactorHabilitado" boolean NOT NULL,
        "LockoutEnd" timestamp with time zone,
        "LockoutEnabled" boolean NOT NULL,
        "CantidadDeAccesosFallidos" integer NOT NULL,
        CONSTRAINT "PK_Usuario" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "ValidacionEnrrolamiento" (
        "Id" uuid NOT NULL,
        "IdValidado_Usuario" uuid NOT NULL,
        "IdValidaEnrrolamiento_Usuario" uuid NOT NULL,
        "EnrrolamientoAceptado" boolean NOT NULL,
        "FechaValidacion" timestamp with time zone NOT NULL,
        "FechaRegistro" timestamp with time zone NOT NULL,
        "Activo" boolean NOT NULL,
        CONSTRAINT "PK_ValidacionEnrrolamiento_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "AspNetRoleClaims" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "RoleId" text NOT NULL,
        "ClaimType" text,
        "ClaimValue" text,
        CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetRoleClaims_Rol_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Rol" ("Id_Rol") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "AspNetUserClaims" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "UserId" text NOT NULL,
        "ClaimType" text,
        "ClaimValue" text,
        CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetUserClaims_Usuario_UserId" FOREIGN KEY ("UserId") REFERENCES "Usuario" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "AspNetUserLogins" (
        "LoginProvider" text NOT NULL,
        "ProviderKey" text NOT NULL,
        "ProviderDisplayName" text,
        "UserId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
        CONSTRAINT "FK_AspNetUserLogins_Usuario_UserId" FOREIGN KEY ("UserId") REFERENCES "Usuario" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "AspNetUserRoles" (
        "UserId" text NOT NULL,
        "RoleId" text NOT NULL,
        CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
        CONSTRAINT "FK_AspNetUserRoles_Rol_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Rol" ("Id_Rol") ON DELETE CASCADE,
        CONSTRAINT "FK_AspNetUserRoles_Usuario_UserId" FOREIGN KEY ("UserId") REFERENCES "Usuario" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE TABLE "AspNetUserTokens" (
        "UserId" text NOT NULL,
        "LoginProvider" text NOT NULL,
        "Name" text NOT NULL,
        "Value" text,
        CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
        CONSTRAINT "FK_AspNetUserTokens_Usuario_UserId" FOREIGN KEY ("UserId") REFERENCES "Usuario" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "AutenticadorExterno" ("Id", "Activo", "AutenticadorExternoBase", "ClaveDeAcceso", "Id_Proveedor", "Id_Usuario", "NombreADesplegar", "NombreUsuario", "ValidadorPrimario")
    VALUES ('ecaf1074-722d-468f-81fa-69c2d7b88d68', TRUE, TRUE, 'AQAAAAIAAYagAAAAECdJ1ZnZCqaIsQ6GpLFpnLkW+Cg2cBy4XTdcpjto7RxHXfKjd0SOs884Ak8Ag9pw2Q==', '701c19bf-405c-4467-85f0-ddbc3786f9ee', '2b12d04f-c167-4ad1-a42a-e2ecd30518d7', 'ADMINISTRADOR', 'ADMINISTRADOR', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "Entidad" ("Id", "CorreoElectronico", "EntidadBase", "FechaCreacion", "FechaInicioAutorizacion", "FechaTerminoAutorizacion", "Id_UnidadOrganizacional", "Id_Usuario", "Principal", "TipoDeEntidad")
    VALUES ('05507441-5792-4c46-9334-9a5faa99e20a', '', TRUE, TIMESTAMPTZ '2025-12-03T11:36:50.732871-03:00', TIMESTAMPTZ '-infinity', TIMESTAMPTZ 'infinity', '198c164d-1cd8-4107-9db3-74b9fa33302c', '2b12d04f-c167-4ad1-a42a-e2ecd30518d7', TRUE, 'UNIDAD_ORGANIZACIONAL');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "Organizacion" ("Id", "Activo", "Codigo", "Descripcion", "IdOrganizacion", "Nombre", "OrganizacionBase")
    VALUES ('70c699b4-eb37-49a4-9dcf-1fc87be16489', TRUE, 'ANID', 'Entidad pública encargada de promover la investigación y el desarrollo en Chile.', '', 'Agencia Nacional de Investigación y Desarrollo', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb55', TIMESTAMPTZ '2025-12-03T14:36:50.733214+00:00', TIMESTAMPTZ '-infinity', TIMESTAMPTZ 'infinity', '05507441-5792-4c46-9334-9a5faa99e20a', 'd1889c7c-c5dc-4d9a-a2fe-34cdf956b145', '856a08fd-4162-47cb-bf92-ff25029f3546', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('8f589ba2-3bc0-40ea-b7c2-7aaaee278d6d', TIMESTAMPTZ '2025-12-03T14:36:50.733105+00:00', TIMESTAMPTZ '-infinity', TIMESTAMPTZ 'infinity', '05507441-5792-4c46-9334-9a5faa99e20a', 'd1889c7c-c5dc-4d9a-a2fe-34cdf956b145', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('bf05f4af-4bbc-472f-a7ed-bbd6d5d1af61', TIMESTAMPTZ '2025-12-03T14:36:50.733214+00:00', TIMESTAMPTZ '-infinity', TIMESTAMPTZ 'infinity', '05507441-5792-4c46-9334-9a5faa99e20a', 'd1889c7c-c5dc-4d9a-a2fe-34cdf956b145', '03b6b706-a24f-4505-9ef6-e3ae7d48c907', TRUE, TRUE, FALSE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('06001a21-9b5f-47a3-ae8b-c749e531f9b1', TRUE, 'SEGUIMIENTO_FINANCIERO', 'IFRAME', 'Sistema de Seguimiento Financiero', 'Sistema de Seguimiento Financiero', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Seguimiento Financiero', TRUE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('233783de-9094-4030-907d-82d7c5abf10e', TRUE, 'POSTULACION', 'IFRAME', 'Sistema de Postulaciones', 'Proceso de Postulacion', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Proceso de Postulacion', TRUE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('2b597b09-55ad-4304-b57d-76bd2df5ac4c', TRUE, 'SFI_RENDICIONES', 'IFRAME', 'Sistema de Seguimiento Financiero Rendiciones', 'Sistema de Seguimiento Financiero Rendiciones', '06001a21-9b5f-47a3-ae8b-c749e531f9b1', 1, 'NIVEL_SISTEMA', 'Sistema de Seguimiento Financiero Rendiciones', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('3f5cceaf-4c86-4495-90b4-aafe7c4ab509', TRUE, 'POS_POSTULAR', 'IFRAME', 'Sistema de Postulaciones Postular', 'Proceso de Postulacion Postular', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Proceso de Postulacion Postular', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('4bb69d17-cc38-4e52-ad36-42226aa5723d', TRUE, 'PSC_REPOSITORIO_ANID', 'IFRAME', 'Sistema de Productividad Científica Repositorio ANID', 'Sistema de Productividad Científica Repositorio ANID', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema de Productividad Científica Repositorio ANID', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('5eeb676d-b3fc-4db7-9421-358a6c26d3dc', TRUE, 'SFO_FALLO', 'IFRAME', 'Sistema de Seleccion y Autorizacion Fallo', 'Sistema de Seleccion y Autorizacion Fallo', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema de Seleccion y Autorizacion Fallo', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('6bd742e3-0e0a-4990-b51f-661c710ba4b9', TRUE, 'SFO_ADMISIBILIDAD', 'IFRAME', 'Sistema de Seleccion y Autorizacion Admisibilidad', 'Sistema de Seleccion y Autorizacion Admisibilidad', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema de Seleccion y Autorizacion Admisibilidad', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('7f618ddf-cadc-4955-b97f-31ebb43cac6f', TRUE, 'EXP_EXPEDIENTE_ELECTRONICO', 'IFRAME', 'Sistema de Expediente Electrónico Expediente', 'Sistema de Expediente Electrónico Expediente', 'ed685882-0d73-4fe4-986c-b34f9641622c', 1, 'NIVEL_SISTEMA', 'Sistema de Expediente Electrónico Expediente', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('7faa600f-2f47-4168-9a65-3c6d47ef2241', TRUE, 'SFO_EVALUACION', 'IFRAME', 'Sistema de Seleccion y Autorizacion Evaluacion', 'Sistema de Seleccion y Autorizacion Evaluacion', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema de Seleccion y Autorizacion Evaluacion', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('a4ebe253-17c4-4a98-a333-d6fee919a212', TRUE, 'PSC_DATOS_ABIERTOS', 'IFRAME', 'Sistema de Productividad Científica Datos Abiertos', 'Sistema de Productividad Científica Datos Abiertos', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema de Productividad Científica Datos Abiertos', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('a6ef77f1-e26b-430d-9d94-eef157e4df65', TRUE, 'POS_CARTAS_DE_RECOMENDACION', 'IFRAME', 'Sistema de Postulaciones Cartas de recomendación', 'Proceso de Postulacion Cartas de recomendación', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Proceso de Postulacion Cartas de recomendación', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('aeaeb19b-2206-4870-9a99-f5d40a982b2e', TRUE, 'SEGUIMIENTO_TECNICO', 'IFRAME', 'Sistema de Seguimiento Técnico', 'Sistema de Seguimiento Técnico', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Seguimiento Técnico', TRUE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('afc327c0-3970-40c7-9ef6-c0a3fdb42cb9', TRUE, 'POS_CONVOCATORIA', 'IFRAME', 'Sistema de Postulaciones Convocatoria', 'Proceso de Postulacion Convocatoria', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Proceso de Postulacion Convocatoria', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('c4a10de0-791c-45ec-820c-1a8802cd3e80', TRUE, 'SELECCION_FORMALIZACION', 'IFRAME', 'Sistema de Seleccion y Autorizacion', 'Sistema de Seleccion y Autorizacion', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Seleccion y Autorizacion', TRUE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('c6b401ab-5164-43cc-8ae4-28ca2c9edbbe', TRUE, 'POS_PATROCINIO_INSTITUCIONAL', 'IFRAME', 'Sistema de Postulaciones Patrocinio Institucional', 'Proceso de Postulacion Patrocinio Institucional', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Proceso de Postulacion Patrocinio Institucional', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('d1889c7c-c5dc-4d9a-a2fe-34cdf956b145', TRUE, 'ADMINISTRACION', 'IFRAME', 'Sistema de Administtracion de Permisos', 'Manejo y asignacion de autorizaciones a los usuarios', '00000000-0000-0000-0000-000000000000', 0, 'NIVEL_MACRO', 'Sistema de Autorización', TRUE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('e256405c-0bda-479a-8a41-a043c672f9b1', TRUE, 'SFI_PROYECTOS_PRESUPUESTO', 'IFRAME', 'Sistema de Seguimiento Financiero Proyectos y Presupuesto', 'Sistema de Seguimiento Financiero Proyectos y Presupuesto', '06001a21-9b5f-47a3-ae8b-c749e531f9b1', 1, 'NIVEL_SISTEMA', 'Sistema de Seguimiento Financiero Proyectos y Presupuesto', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('ed685882-0d73-4fe4-986c-b34f9641622c', TRUE, 'EXPEDIENTE', 'IFRAME', 'Sistema de Expediente Electrónico', 'Sistema de Expediente Electrónico', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Expediente Electrónico', TRUE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('eec159f1-9ba9-463d-8407-ec2951751c29', TRUE, 'PSC_PORTAL_DEL_INVESTIGADOR', 'IFRAME', 'Sistema de Productividad Científica Portal del Investigador', 'Sistema de Productividad Científica Portal del Investigador', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema de Productividad Científica Portal del Investigador', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('f2224186-ffcc-41ee-bbce-4bbd72504e22', TRUE, 'STE_PROYECTOS_INFORMES', 'IFRAME', 'Sistema de Seguimiento Técnico Proyectos e Informes', 'Sistema de Seguimiento Técnico Proyectos e Informes', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 1, 'NIVEL_SISTEMA', 'Sistema de Seguimiento Técnico Proyectos e Informes', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('f78ccd33-9762-4beb-83bc-7f2b02a295d7', TRUE, 'SFO_FIRMA_CONVENIO', 'IFRAME', 'Sistema de Seleccion y Autorizacion Firma de Convenio', 'Sistema de Seleccion y Autorizacion Firma de Convenio', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema de Seleccion y Autorizacion Firma de Convenio', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', TRUE, 'PRODUCTIVIDAD_CIENTÍFICA', 'IFRAME', 'Sistema de Productividad Científica', 'Sistema de Productividad Científica', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Productividad Científica', TRUE, '', 'http://localhost:4210');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "Proveedor" ("Id", "APIDeAutenticacion", "Activo", "Codigo", "Descripcion", "Nombre", "ProveedorBase")
    VALUES ('701c19bf-405c-4467-85f0-ddbc3786f9ee', 'https://www.aut2.darksidetech.services/', TRUE, 'ANID', 'Proveedor de autenticación implementadop or ANID.', 'ANID', TRUE);
    INSERT INTO "Proveedor" ("Id", "APIDeAutenticacion", "Activo", "Codigo", "Descripcion", "Nombre", "ProveedorBase")
    VALUES ('9d9b40fa-fecb-41f6-ad4f-8ed7ceb1be13', 'https://api.claveunica.gob.cl', TRUE, 'CLAVEUNICA', 'Proveedor de autenticación del gobierno chileno para servicios públicos.', 'Clave Única', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('03b6b706-a24f-4505-9ef6-e3ae7d48c907', FALSE, TRUE, NULL, '', 'VALIDA_ASIGNACION_ROLES', 'VALIDA_ASIGNACION_ROLES', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('36957ec2-2857-4101-a81b-f0340bf8eff2', FALSE, TRUE, NULL, '', 'ADMINISTRADOR_ENTIDAD', 'ADMINISTRADOR_ENTIDAD', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('856a08fd-4162-47cb-bf92-ff25029f3546', FALSE, TRUE, NULL, '', 'VALIDA_ENRROLAMIENTO', 'VALIDA_ENRROLAMIENTO', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', FALSE, TRUE, NULL, '', 'ADMINISTRADOR', 'ADMINISTRADOR', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('e198ec28-2b1b-48b0-8d5e-eb946d596e90', FALSE, TRUE, NULL, '', 'USUARIO', 'USUARIO', FALSE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('e3093727-b36b-45af-a495-7c3d0804c0e9', FALSE, TRUE, NULL, '', 'ADMINISTRADOR_UNIDAD', 'ADMINISTRADOR_UNIDAD', TRUE, FALSE, TRUE, FALSE, FALSE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "UnidadOrganizacional" ("Id", "Activo", "Codigo", "Descripcion", "Id_Organizacion", "Nombre", "UnidadOrganizacionalBase")
    VALUES ('198c164d-1cd8-4107-9db3-74b9fa33302c', TRUE, 'CASA_MATRIZ', 'Casa Matriz de la Agencia Nacional de Investigación y Desarrollo', '70c699b4-eb37-49a4-9dcf-1fc87be16489', 'Casa Matriz ANID', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "Usuario" ("Id", "CantidadDeAccesosFallidos", "Activo", "ConcurrencyStamp", "Descripcion", "CorreoElectronico", "CorreoElectronicoConfirmado", "EstadoDeUsuario", "IdPersona", "InformacionAdicional", "LockoutEnabled", "LockoutEnd", "NombreADesplegar", "CorreoElectronicoNormalizado", "NombreUsuarioNormalizado", "HashDeLaClave", "NumeroDeTelefono", "NumeroDeTelefonoConfirmado", "RefreshToken", "RefreshTokenExpiresAtUtc", "RequiereValidacionEnrrolamiento", "SecurityStamp", "TipoDeUsuario", "DobleFactorHabilitado", "NombreUsuario", "UsuarioBase")
    VALUES ('2b12d04f-c167-4ad1-a42a-e2ecd30518d7', 10, TRUE, '33ef92f3-eac5-4019-923e-d1bf66d701ed', 'Administrador global', 'administrador@security.com', TRUE, 'REGISTRADO', '', '', FALSE, NULL, 'Administrador', 'ADMINISTRADOR@SECURITY.COM', 'ADMINISTRADOR', 'AQAAAAIAAYagAAAAECdJ1ZnZCqaIsQ6GpLFpnLkW+Cg2cBy4XTdcpjto7RxHXfKjd0SOs884Ak8Ag9pw2Q==', '', TRUE, NULL, NULL, FALSE, '', 'NACIONAL', FALSE, 'ADMINISTRADOR', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "Rol" ("NombreNormalizado");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE INDEX "EmailIndex" ON "Usuario" ("CorreoElectronicoNormalizado");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "Usuario" ("NombreUsuarioNormalizado");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20251203143651_Inicial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20251203143651_Inicial', '9.0.8');
    END IF;
END $EF$;
COMMIT;

