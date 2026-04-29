CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE TABLE "Entidad" (
        "Id" uuid NOT NULL,
        "Id_UnidadOrganizacional" uuid NOT NULL,
        "Id_Usuario" uuid NOT NULL,
        "TipoDeEntidad" character varying(100) NOT NULL,
        "CorreoElectronico" text NOT NULL,
        "FechaInicioAutorizacion" timestamp with time zone,
        "FechaTerminoAutorizacion" timestamp with time zone,
        "FechaCreacion" timestamp with time zone NOT NULL,
        "Principal" boolean NOT NULL,
        "EntidadBase" boolean NOT NULL,
        CONSTRAINT "PK_Entidad_Id" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE TABLE "PoliticaAsignada" (
        "Id" uuid NOT NULL,
        "Id_Entidad" uuid NOT NULL,
        "Id_Rol" uuid NOT NULL,
        "Id_Proceso" uuid NOT NULL,
        "FechaInicioAsignacion" timestamp with time zone,
        "FechaTerminoAsignacion" timestamp with time zone,
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
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
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE TABLE "RefreshTokens" (
        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
        "SessionId" character varying(64) NOT NULL,
        "TokenHash" character varying(128) NOT NULL,
        "CreatedAtUtc" timestamp with time zone,
        "ExpiresAtUtc" timestamp with time zone,
        "RevokedAtUtc" timestamp with time zone,
        "SelectedOrganization" character varying(256),
        "ReplacedByTokenHash" character varying(128),
        "RevocationReason" character varying(64),
        "UserId" text NOT NULL,
        "Id_Entidad" uuid NOT NULL,
        CONSTRAINT "PK_RefreshTokens" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_RefreshTokens_Usuario_UserId" FOREIGN KEY ("UserId") REFERENCES "Usuario" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "AutenticadorExterno" ("Id", "Activo", "AutenticadorExternoBase", "ClaveDeAcceso", "Id_Proveedor", "Id_Usuario", "NombreADesplegar", "NombreUsuario", "ValidadorPrimario")
    VALUES ('ecaf1074-722d-468f-81fa-69c2d7b88d68', TRUE, TRUE, 'AQAAAAIAAYagAAAAEL23Xp7j+JxUsrKAljUnZ89wxmX/rYfRl8mrMVy20i8pAO1tng9S7zJYwpOucvcyrg==', '701c19bf-405c-4467-85f0-ddbc3786f9ee', '2b12d04f-c167-4ad1-a42a-e2ecd30518d7', 'ADMINISTRADOR', 'ADMINISTRADOR', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "Entidad" ("Id", "CorreoElectronico", "EntidadBase", "FechaCreacion", "FechaInicioAutorizacion", "FechaTerminoAutorizacion", "Id_UnidadOrganizacional", "Id_Usuario", "Principal", "TipoDeEntidad")
    VALUES ('05507441-5792-4c46-9334-9a5faa99e20a', '', TRUE, TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '198c164d-1cd8-4107-9db3-74b9fa33302c', '2b12d04f-c167-4ad1-a42a-e2ecd30518d7', TRUE, 'PERSONA');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "Organizacion" ("Id", "Activo", "Codigo", "Descripcion", "IdOrganizacion", "Nombre", "OrganizacionBase")
    VALUES ('70c699b4-eb37-49a4-9dcf-1fc87be16489', TRUE, 'ANID', 'Entidad pública encargada de promover la investigación y el desarrollo en Chile.', '', 'Agencia Nacional de Investigación y Desarrollo', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb55', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'd1889c7c-c5dc-4d9a-a2fe-34cdf956b145', '856a08fd-4162-47cb-bf92-ff25029f3546', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb56', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb57', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'eec159f1-9ba9-463d-8407-ec2951751c29', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb58', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', '4bb69d17-cc38-4e52-ad36-42226aa5723d', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb59', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', '35e9a345-dda9-45fc-9a35-7cf27ec8e947', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb5a', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', '06001a21-9b5f-47a3-ae8b-c749e531f9b1', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb5b', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'e256405c-0bda-479a-8a41-a043c672f9b1', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb5c', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', '2b597b09-55ad-4304-b57d-76bd2df5ac4c', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb5d', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', '0737dfc9-8c0f-44ed-ada9-0d227d6a4b5c', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb5e', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb5f', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'f2224186-ffcc-41ee-bbce-4bbd72504e22', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb60', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'f3285b94-90a4-4610-8c65-2782d2e3a1a3', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb61', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'e150709d-4fed-480b-8916-8b0837a775d3', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('8f589ba2-3bc0-40ea-b7c2-7aaaee278d6d', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'd1889c7c-c5dc-4d9a-a2fe-34cdf956b145', 'c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', TRUE, TRUE, FALSE);
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('bf05f4af-4bbc-472f-a7ed-bbd6d5d1af61', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'd1889c7c-c5dc-4d9a-a2fe-34cdf956b145', '03b6b706-a24f-4505-9ef6-e3ae7d48c907', TRUE, TRUE, FALSE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('06001a21-9b5f-47a3-ae8b-c749e531f9b1', TRUE, 'SEGUIMIENTO_FINANCIERO', 'IFRAME', 'Sistema de Seguimiento Financiero', 'Sistema de Seguimiento Financiero', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Seguimiento Financiero', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('0737dfc9-8c0f-44ed-ada9-0d227d6a4b5c', TRUE, 'SFI_SISTEMA_TERMINO_SIA', 'IFRAME', 'Sistema Sistema Termino SIA', 'Sistema Sistema Termino SIA', '06001a21-9b5f-47a3-ae8b-c749e531f9b1', 1, 'NIVEL_SISTEMA', 'Sistema Sistema Termino SIA', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('233783de-9094-4030-907d-82d7c5abf10e', TRUE, 'POSTULACION', 'IFRAME', 'Sistema de Postulaciones', 'Proceso de Postulacion', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Proceso de Postulacion', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('2b597b09-55ad-4304-b57d-76bd2df5ac4c', TRUE, 'SFI_SISFON_LUTHIEN_SPI_SCH', 'IFRAME', 'Sistema Sisfon Luthien SPI SCH', 'Sistema Sisfon Luthien SPI SCH', '06001a21-9b5f-47a3-ae8b-c749e531f9b1', 1, 'NIVEL_SISTEMA', 'Sistema Sisfon Luthien SPI SCH', FALSE, '', '172.16.4.107:22');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('35e9a345-dda9-45fc-9a35-7cf27ec8e947', TRUE, 'VIN_DATACIENCIA', 'IFRAME', 'Sistema DataCiencia', 'Sistema DataCiencia', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema DataCiencia', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('3f5cceaf-4c86-4495-90b4-aafe7c4ab509', TRUE, 'POS_ACONCAGUA_GENERICO', 'REDIRECCION', 'Sistema Aconcagua generico', 'Sistema Aconcagua Generico', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Sistema Aconcagua Generico', FALSE, '', 'http://auth-qa05.anid.cl');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('4bb69d17-cc38-4e52-ad36-42226aa5723d', TRUE, 'VIN_BEIC', 'IFRAME', 'Sistema Beic', 'Sistema Beic', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema Beic', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('587ec39b-e7d8-4c0f-9ac4-697940cf7b07', TRUE, 'VIN_DIODI', 'IFRAME', 'Sistema DIODI', 'Sistema DIODI', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema DIODI', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('5eeb676d-b3fc-4db7-9421-358a6c26d3dc', TRUE, 'SFO_EVAL_GENERICO', 'VENTANA', 'Sistema Eval Generico', 'Sistema Eval Generico', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema Eval Generico', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('6bd742e3-0e0a-4990-b51f-661c710ba4b9', TRUE, 'SFO_EVAL_SPI', 'VENTANA', 'Sistema Eval SPI', 'Sistema Eval SPI', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema Eval SPI', FALSE, '', 'https://faraondesa.anid.cl/desa2/Evaluacion_TESTING/index.php');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('7faa600f-2f47-4168-9a65-3c6d47ef2241', TRUE, 'SFO_EVAL_BECAS', 'VENTANA', 'Sistema Eval Becas', 'Sistema Eval Becas', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema Eval Becas', FALSE, '', 'https://servicios-qa.anid.cl/evalbecas');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('8ff51db1-44a5-4b9e-b0eb-0cd739cee604', TRUE, 'STE_SISTEMA_VERDE_SIA', 'IFRAME', 'Sistema Verde SIA', 'Sistema Verde SIA', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 1, 'NIVEL_SISTEMA', 'Sistema Verde SIA', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('9679412f-ae9f-4d77-8629-7559d280ece2', TRUE, 'SFO_FALLO_BECA', 'IFRAME', 'Sistema Fallo Beca', 'Sistema Fallo Beca', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema Fallo Beca', FALSE, '', 'https://servicios-qa.anid.cl/web//fallo/#/public');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('9c4a10ea-d6f3-4a0c-b4b0-25fb0396c7ee', TRUE, 'VIN_REPOSITORIO', 'IFRAME', 'Sistema Repositorio', 'Sistema Repositorio', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema Repositorio', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('9cc3ac0d-503a-42b1-bc8e-10d20265e15b', TRUE, 'STE_SISTEMA_TERMINO_SIA', 'IFRAME', 'Sistema Termino SIA', 'Sistema Termino SIA', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 1, 'NIVEL_SISTEMA', 'Sistema Termino SIA', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('a4ebe253-17c4-4a98-a333-d6fee919a212', TRUE, 'VIN_PDI', 'IFRAME', 'Sistema PDI', 'Sistema PDI', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema PDI', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('a6151eed-ea81-4edb-9c89-5bf7e260989f', TRUE, 'SFO_FALLO_SPI', 'IFRAME', 'Sistema Fallo SPI', 'Sistema Fallo SPI', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema Fallo SPI', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('a6ef77f1-e26b-430d-9d94-eef157e4df65', TRUE, 'POS_MILENIO', 'REDIRECCION', 'Sistema de Postulaciones Milenio', 'Proceso de Postulacion Milenio', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Proceso de Postulacion Milenio', FALSE, '', 'https://post-im.conicyt.cl/Concursos');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('aeaeb19b-2206-4870-9a99-f5d40a982b2e', TRUE, 'SEGUIMIENTO_TECNICO', 'IFRAME', 'Sistema de Seguimiento Técnico', 'Sistema de Seguimiento Técnico', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Seguimiento Técnico', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('afc327c0-3970-40c7-9ef6-c0a3fdb42cb9', TRUE, 'POS_ACONCAGUA_SPI', 'REDIRECCION', 'Sistema Aconcagua SPI', 'Sistema Aconcagua SPI', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Sistema Aconcagua SPI', FALSE, '', 'https://auth-qa01.anid.cl');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('c4a10de0-791c-45ec-820c-1a8802cd3e80', TRUE, 'SELECCION_FORMALIZACION', 'IFRAME', 'Sistema de Seleccion y Formalizacion', 'Sistema de Seleccion y Formalizacion', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema de Seleccion y Formalizacion', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('c6b401ab-5164-43cc-8ae4-28ca2c9edbbe', TRUE, 'POS_GENESIS', 'REDIRECCION', 'Sistema Genesis', 'Sistema Genesis', '233783de-9094-4030-907d-82d7c5abf10e', 1, 'NIVEL_SISTEMA', 'Sistema Genesis', FALSE, '', 'https://splqa.anid.cl');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('d1889c7c-c5dc-4d9a-a2fe-34cdf956b145', TRUE, 'ADMINISTRACION', 'IFRAME', 'Sistema de Administtracion de Permisos', 'Manejo y asignacion de autorizaciones a los usuarios', '00000000-0000-0000-0000-000000000000', 0, 'NIVEL_MACRO', 'Sistema de Autorización', TRUE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('d65b2a0d-0742-491b-be4f-c7c00219821d', TRUE, 'SFI_SYC_FINANCIERO_SIA', 'IFRAME', 'Sistema SyC Financiero SIA', 'Sistema SyC Financiero SIA', '06001a21-9b5f-47a3-ae8b-c749e531f9b1', 1, 'NIVEL_SISTEMA', 'Sistema SyC Financiero SIA', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('e150709d-4fed-480b-8916-8b0837a775d3', TRUE, 'STE_GESTION_MILENIO', 'IFRAME', 'Sistema Gestion Milenio', 'Sistema Gestion Milenio', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 1, 'NIVEL_SISTEMA', 'Sistema Gestion Milenio', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('e256405c-0bda-479a-8a41-a043c672f9b1', TRUE, 'SFI_SGDL_SPI', 'IFRAME', 'Sistema SDGL SPI', 'Sistema SDGL SPI', '06001a21-9b5f-47a3-ae8b-c749e531f9b1', 1, 'NIVEL_SISTEMA', 'Sistema SDGL SPI', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('eec159f1-9ba9-463d-8407-ec2951751c29', TRUE, 'VIN_SCIELO', 'IFRAME', 'Sistema Scielo', 'Sistema Scielo', 'fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', 1, 'NIVEL_SISTEMA', 'Sistema Scielo', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('f2224186-ffcc-41ee-bbce-4bbd72504e22', TRUE, 'STE_SIAL_SPI', 'IFRAME', 'Sistema SIAL SPI', 'Sistema SIAL SPI', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 1, 'NIVEL_SISTEMA', 'Sistema SIAL SPI', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('f3285b94-90a4-4610-8c65-2782d2e3a1a3', TRUE, 'STE_SyC_LEGACY_SIA', 'IFRAME', 'Sistema SyC Legacy SIA', 'Sistema SyC Legacy SIA', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 1, 'NIVEL_SISTEMA', 'Sistema SyC Legacy SIA', FALSE, '', 'http://localhost:4210');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('f78ccd33-9762-4beb-83bc-7f2b02a295d7', TRUE, 'SFO_FIRMA_CONVENIO', 'IFRAME', 'Sistema de Seleccion y Autorizacion Firma de Convenio', 'Sistema de Seleccion y Autorizacion Firma de Convenio', 'c4a10de0-791c-45ec-820c-1a8802cd3e80', 1, 'NIVEL_SISTEMA', 'Sistema de Seleccion y Autorizacion Firma de Convenio', FALSE, '', 'https://servicios-qa.anid.cl/web/firma-convenio/#/login');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('fa2e0bda-3063-4357-91b5-17f443b74ebd', TRUE, 'STE_SISFON_LUTHIEN_SPI_SCH', 'IFRAME', 'Sistema Sisfon Luthien SPI SCH', 'Sistema Sisfon Luthien SPI SCH', 'aeaeb19b-2206-4870-9a99-f5d40a982b2e', 1, 'NIVEL_SISTEMA', 'Sistema Sisfon Luthien SPI SCH', FALSE, '', '172.16.4.107:22');
    INSERT INTO "Proceso" ("Id", "Activo", "Codigo", "ComoDesplegarUrlDeProceso", "Contexto", "Descripcion", "IdMacro_Proceso", "MaximaAsignacionDeRoles", "NivelDeProceso", "Nombre", "ProcesoBase", "Token", "Url")
    VALUES ('fcd2dcf8-7230-4fdd-9651-b4efeb60d11f', TRUE, 'VIN_SCIELO', 'IFRAME', 'Sistema Scielo', 'Sistema Scielo', '00000000-0000-0000-0000-000000000000', 1, 'NIVEL_MACRO', 'Sistema Scielo', FALSE, '', 'http://localhost:4210');
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "Proveedor" ("Id", "APIDeAutenticacion", "Activo", "Codigo", "Descripcion", "Nombre", "ProveedorBase")
    VALUES ('701c19bf-405c-4467-85f0-ddbc3786f9ee', 'https://www.aut2.darksidetech.services/', TRUE, 'ANID', 'Proveedor de autenticación implementadop or ANID.', 'ANID', TRUE);
    INSERT INTO "Proveedor" ("Id", "APIDeAutenticacion", "Activo", "Codigo", "Descripcion", "Nombre", "ProveedorBase")
    VALUES ('9d9b40fa-fecb-41f6-ad4f-8ed7ceb1be13', 'https://api.claveunica.gob.cl', TRUE, 'CLAVEUNICA', 'Proveedor de autenticación del gobierno chileno para servicios públicos.', 'Clave Única', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('03b6b706-a24f-4505-9ef6-e3ae7d48c907', FALSE, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d002004', '', 'VALIDA_ASIGNACION_ROLES', 'VALIDA_ASIGNACION_ROLES', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('36957ec2-2857-4101-a81b-f0340bf8eff2', FALSE, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d002002', '', 'ADMINISTRADOR_ENTIDAD', 'ADMINISTRADOR_ENTIDAD', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('856a08fd-4162-47cb-bf92-ff25029f3546', FALSE, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d002005', '', 'VALIDA_ENRROLAMIENTO', 'VALIDA_ENRROLAMIENTO', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e', FALSE, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d002001', '', 'ADMINISTRADOR', 'ADMINISTRADOR', TRUE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('e198ec28-2b1b-48b0-8d5e-eb946d596e90', FALSE, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d002006', '', 'USUARIO', 'USUARIO', FALSE, FALSE, TRUE, FALSE, FALSE);
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('e3093727-b36b-45af-a495-7c3d0804c0e9', FALSE, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d002003', '', 'ADMINISTRADOR_UNIDAD', 'ADMINISTRADOR_UNIDAD', TRUE, FALSE, TRUE, FALSE, FALSE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "UnidadOrganizacional" ("Id", "Activo", "Codigo", "Descripcion", "Id_Organizacion", "Nombre", "UnidadOrganizacionalBase")
    VALUES ('198c164d-1cd8-4107-9db3-74b9fa33302c', TRUE, 'CASA_MATRIZ', 'Casa Matriz de la Agencia Nacional de Investigación y Desarrollo', '70c699b4-eb37-49a4-9dcf-1fc87be16489', 'Casa Matriz ANID', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "Usuario" ("Id", "CantidadDeAccesosFallidos", "Activo", "ConcurrencyStamp", "Descripcion", "CorreoElectronico", "CorreoElectronicoConfirmado", "EstadoDeUsuario", "IdPersona", "InformacionAdicional", "LockoutEnabled", "LockoutEnd", "NombreADesplegar", "CorreoElectronicoNormalizado", "NombreUsuarioNormalizado", "HashDeLaClave", "NumeroDeTelefono", "NumeroDeTelefonoConfirmado", "RefreshToken", "RefreshTokenExpiresAtUtc", "RequiereValidacionEnrrolamiento", "SecurityStamp", "TipoDeUsuario", "DobleFactorHabilitado", "NombreUsuario", "UsuarioBase")
    VALUES ('2b12d04f-c167-4ad1-a42a-e2ecd30518d7', 10, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d001001', 'Administrador global', 'administrador@security.com', TRUE, 'REGISTRADO', '', '', FALSE, NULL, 'Administrador', 'ADMINISTRADOR@SECURITY.COM', 'ADMINISTRADOR', 'AQAAAAIAAYagAAAAEL23Xp7j+JxUsrKAljUnZ89wxmX/rYfRl8mrMVy20i8pAO1tng9S7zJYwpOucvcyrg==', '', TRUE, NULL, NULL, FALSE, '', 'NACIONAL', FALSE, 'ADMINISTRADOR', TRUE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE INDEX "IX_RefreshTokens_SessionId" ON "RefreshTokens" ("SessionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE UNIQUE INDEX "IX_RefreshTokens_TokenHash" ON "RefreshTokens" ("TokenHash");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE INDEX "IX_RefreshTokens_UserId" ON "RefreshTokens" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "Rol" ("NombreNormalizado");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE INDEX "EmailIndex" ON "Usuario" ("CorreoElectronicoNormalizado");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "Usuario" ("NombreUsuarioNormalizado");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260408205139_Inicial') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260408205139_Inicial', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE TABLE "AuditAggregateCursor" (
        "AggregateId" uuid NOT NULL,
        "LastRevision" bigint NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_AuditAggregateCursor" PRIMARY KEY ("AggregateId")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE TABLE "AuditOutbox" (
        "Id" uuid NOT NULL,
        "CorrelationId" uuid NOT NULL,
        "AggregateId" uuid NOT NULL,
        "AggregateType" character varying(150) NOT NULL,
        "AggregateRevision" bigint NOT NULL,
        "EventType" character varying(200) NOT NULL,
        "CommandType" character varying(200) NOT NULL,
        "OperationType" integer NOT NULL,
        "ActorUserId" character varying(100),
        "ActorUsername" character varying(256),
        "ActorEmail" character varying(256),
        "RequestPath" character varying(512),
        "OccurredAtUtc" timestamp with time zone NOT NULL,
        "PersistedAtUtc" timestamp with time zone NOT NULL,
        "SnapshotJson" text,
        "DispatchStatus" smallint NOT NULL DEFAULT 0,
        "DispatchAttempts" integer NOT NULL DEFAULT 0,
        "LastDispatchAttemptUtc" timestamp with time zone,
        "DispatchedAtUtc" timestamp with time zone,
        "LastError" text,
        "SchemaVersion" smallint NOT NULL DEFAULT 1,
        CONSTRAINT "PK_AuditOutbox" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE TABLE "AuditOutboxChange" (
        "Id" uuid NOT NULL,
        "AuditOutboxMessageId" uuid NOT NULL,
        "Order" integer NOT NULL,
        "Path" character varying(200) NOT NULL,
        "ValueType" character varying(512),
        "NewValueJson" text,
        CONSTRAINT "PK_AuditOutboxChange" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AuditOutboxChange_AuditOutbox_AuditOutboxMessageId" FOREIGN KEY ("AuditOutboxMessageId") REFERENCES "AuditOutbox" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE INDEX "IX_AuditOutbox_ActorUserId_OccurredAtUtc" ON "AuditOutbox" ("ActorUserId", "OccurredAtUtc");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE UNIQUE INDEX "IX_AuditOutbox_AggregateId_AggregateRevision" ON "AuditOutbox" ("AggregateId", "AggregateRevision");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE INDEX "IX_AuditOutbox_AggregateId_OccurredAtUtc_Id" ON "AuditOutbox" ("AggregateId", "OccurredAtUtc", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE INDEX "IX_AuditOutbox_CorrelationId" ON "AuditOutbox" ("CorrelationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE INDEX "IX_AuditOutbox_DispatchStatus_PersistedAtUtc_Id" ON "AuditOutbox" ("DispatchStatus", "PersistedAtUtc", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE INDEX "IX_AuditOutbox_EventType_OccurredAtUtc" ON "AuditOutbox" ("EventType", "OccurredAtUtc");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE INDEX "IX_AuditOutboxChange_AuditOutboxMessageId" ON "AuditOutboxChange" ("AuditOutboxMessageId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    CREATE INDEX "IX_AuditOutboxChange_Path" ON "AuditOutboxChange" ("Path");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260411163537_AddAuditTraceability') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260411163537_AddAuditTraceability', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260412004336_AddAuditAggregateCursorConcurrencyToken') THEN
    ALTER TABLE "AuditAggregateCursor" ADD "ConcurrencyToken" uuid NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260412004336_AddAuditAggregateCursorConcurrencyToken') THEN
    INSERT INTO "PoliticaAsignada" ("Id", "FechaCreacion", "FechaInicioAsignacion", "FechaTerminoAsignacion", "Id_Entidad", "Id_Proceso", "Id_Rol", "PoliticaAsignadaBase", "RolAsignadoValidado", "RolRequiereValidacion")
    VALUES ('60838d42-c2df-402c-9253-ab3ce52ffb62', TIMESTAMPTZ '2026-04-07T23:04:30+00:00', NULL, NULL, '05507441-5792-4c46-9334-9a5faa99e20a', 'd1889c7c-c5dc-4d9a-a2fe-34cdf956b145', 'e198ec28-2b1b-48b0-8d5e-eb946d596e91', TRUE, TRUE, FALSE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260412004336_AddAuditAggregateCursorConcurrencyToken') THEN
    INSERT INTO "Rol" ("Id_Rol", "ActivaDetalleDeAutorizaciones", "Activo", "ConcurrencyStamp", "Descripcion", "Nombre", "NombreNormalizado", "RequiereAccionParaSerAsignado", "RequiereValidacionDeAsignacion", "RolBase", "ValidaAsignacionDeRoles", "ValidaEnrrolamiento")
    VALUES ('e198ec28-2b1b-48b0-8d5e-eb946d596e91', FALSE, TRUE, '4f4d7775-2677-47e7-8ccd-a34f5d002007', '', 'AUDITOR_TRAZABILIDAD', 'AUDITOR_TRAZABILIDAD', TRUE, FALSE, TRUE, FALSE, FALSE);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260412004336_AddAuditAggregateCursorConcurrencyToken') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260412004336_AddAuditAggregateCursorConcurrencyToken', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260414014337_AddNotificationOutbox') THEN
    CREATE TABLE "NotificationOutbox" (
        "Id" uuid NOT NULL,
        "Channel" character varying(50) NOT NULL,
        "NotificationType" character varying(150) NOT NULL,
        "UserId" character varying(100),
        "RequestPath" character varying(512),
        "DeduplicationKey" character varying(300),
        "PayloadJson" text NOT NULL,
        "DispatchStatus" smallint NOT NULL DEFAULT 0,
        "DispatchAttempts" integer NOT NULL DEFAULT 0,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "NextAttemptUtc" timestamp with time zone NOT NULL,
        "LastDispatchAttemptUtc" timestamp with time zone,
        "DispatchedAtUtc" timestamp with time zone,
        "LastError" text,
        "SchemaVersion" smallint NOT NULL DEFAULT 1,
        CONSTRAINT "PK_NotificationOutbox" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260414014337_AddNotificationOutbox') THEN
    CREATE INDEX "IX_NotificationOutbox_Channel_NotificationType_CreatedAtUtc" ON "NotificationOutbox" ("Channel", "NotificationType", "CreatedAtUtc");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260414014337_AddNotificationOutbox') THEN
    CREATE INDEX "IX_NotificationOutbox_DeduplicationKey" ON "NotificationOutbox" ("DeduplicationKey");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260414014337_AddNotificationOutbox') THEN
    CREATE INDEX "IX_NotificationOutbox_DispatchStatus_NextAttemptUtc_CreatedAtU~" ON "NotificationOutbox" ("DispatchStatus", "NextAttemptUtc", "CreatedAtUtc", "Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260414014337_AddNotificationOutbox') THEN
    CREATE INDEX "IX_NotificationOutbox_UserId" ON "NotificationOutbox" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260414014337_AddNotificationOutbox') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260414014337_AddNotificationOutbox', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260416223834_AddPasswordChangeChallenges') THEN
    CREATE TABLE "PasswordChangeChallenges" (
        "Id" uuid NOT NULL,
        "UserId" character varying(100) NOT NULL,
        "CodeHash" character varying(256) NOT NULL,
        "RequestPath" character varying(512),
        "FailedAttempts" integer NOT NULL DEFAULT 0,
        "ResendCount" integer NOT NULL DEFAULT 0,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "LastSentAtUtc" timestamp with time zone NOT NULL,
        "ExpiresAtUtc" timestamp with time zone NOT NULL,
        "ConsumedAtUtc" timestamp with time zone,
        "CancelledAtUtc" timestamp with time zone,
        CONSTRAINT "PK_PasswordChangeChallenges" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260416223834_AddPasswordChangeChallenges') THEN
    CREATE INDEX "IX_PasswordChangeChallenges_UserId_CreatedAtUtc" ON "PasswordChangeChallenges" ("UserId", "CreatedAtUtc");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260416223834_AddPasswordChangeChallenges') THEN
    CREATE INDEX "IX_PasswordChangeChallenges_UserId_ExpiresAtUtc_ConsumedAtUtc_~" ON "PasswordChangeChallenges" ("UserId", "ExpiresAtUtc", "ConsumedAtUtc", "CancelledAtUtc");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260416223834_AddPasswordChangeChallenges') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260416223834_AddPasswordChangeChallenges', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260424120000_AddPasswordRecoveryChallengePurpose') THEN
    DROP INDEX "IX_PasswordChangeChallenges_UserId_ExpiresAtUtc_ConsumedAtUtc_~";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260424120000_AddPasswordRecoveryChallengePurpose') THEN
    ALTER TABLE "PasswordChangeChallenges" ADD "ChallengePurpose" character varying(50) NOT NULL DEFAULT 'PASSWORD_CHANGE';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260424120000_AddPasswordRecoveryChallengePurpose') THEN
    CREATE INDEX "IX_PasswordChangeChallenges_UserId_Purpose_Expires" ON "PasswordChangeChallenges" ("UserId", "ChallengePurpose", "ExpiresAtUtc", "ConsumedAtUtc", "CancelledAtUtc");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260424120000_AddPasswordRecoveryChallengePurpose') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260424120000_AddPasswordRecoveryChallengePurpose', '10.0.7');
    END IF;
END $EF$;
COMMIT;

