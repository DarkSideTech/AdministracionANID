IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [AutenticadorExterno] (
        [Id] uniqueidentifier NOT NULL,
        [Id_Proveedor] uniqueidentifier NOT NULL,
        [Id_Usuario] uniqueidentifier NOT NULL,
        [NombreUsuario] nvarchar(255) NOT NULL,
        [ClaveDeAcceso] nvarchar(max) NOT NULL,
        [NombreADesplegar] nvarchar(max) NOT NULL,
        [ValidadorPrimario] bit NOT NULL,
        [AutenticadorExternoBase] bit NOT NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_AutenticadorExterno_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [Entidad] (
        [Id] uniqueidentifier NOT NULL,
        [Id_UnidadOrganizacional] uniqueidentifier NOT NULL,
        [Id_Usuario] uniqueidentifier NOT NULL,
        [TipoDeEntidad] nvarchar(100) NOT NULL,
        [CorreoElectronico] nvarchar(max) NOT NULL,
        [FechaInicioAutorizacion] datetimeoffset NULL,
        [FechaTerminoAutorizacion] datetimeoffset NULL,
        [FechaCreacion] datetimeoffset NOT NULL,
        [Principal] bit NOT NULL,
        [EntidadBase] bit NOT NULL,
        CONSTRAINT [PK_Entidad_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [Organizacion] (
        [Id] uniqueidentifier NOT NULL,
        [IdOrganizacion] nvarchar(max) NOT NULL,
        [Codigo] nvarchar(100) NOT NULL,
        [Nombre] nvarchar(255) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [OrganizacionBase] bit NOT NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Organizacion_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [PoliticaAsignada] (
        [Id] uniqueidentifier NOT NULL,
        [Id_Entidad] uniqueidentifier NOT NULL,
        [Id_Rol] uniqueidentifier NOT NULL,
        [Id_Proceso] uniqueidentifier NOT NULL,
        [FechaInicioAsignacion] datetimeoffset NULL,
        [FechaTerminoAsignacion] datetimeoffset NULL,
        [FechaCreacion] datetimeoffset NOT NULL,
        [RolRequiereValidacion] bit NOT NULL,
        [RolAsignadoValidado] bit NOT NULL,
        [PoliticaAsignadaBase] bit NOT NULL,
        CONSTRAINT [PK_PoliticaAsignada_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [Proceso] (
        [Id] uniqueidentifier NOT NULL,
        [IdMacro_Proceso] uniqueidentifier NOT NULL,
        [Codigo] nvarchar(100) NOT NULL,
        [Nombre] nvarchar(255) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [Contexto] nvarchar(max) NOT NULL,
        [NivelDeProceso] nvarchar(100) NOT NULL,
        [Url] nvarchar(max) NOT NULL,
        [Token] nvarchar(max) NOT NULL,
        [ComoDesplegarUrlDeProceso] nvarchar(100) NOT NULL,
        [ProcesoBase] bit NOT NULL,
        [MaximaAsignacionDeRoles] int NOT NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Proceso_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [Proveedor] (
        [Id] uniqueidentifier NOT NULL,
        [Codigo] nvarchar(100) NOT NULL,
        [Nombre] nvarchar(255) NOT NULL,
        [Descripcion] nvarchar(500) NOT NULL,
        [APIDeAutenticacion] nvarchar(max) NOT NULL,
        [ProveedorBase] bit NOT NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_Proveedor_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [Rol] (
        [Id_Rol] nvarchar(450) NOT NULL,
        [Descripcion] nvarchar(max) NULL,
        [RequiereAccionParaSerAsignado] bit NULL,
        [ActivaDetalleDeAutorizaciones] bit NULL,
        [RequiereValidacionDeAsignacion] bit NULL,
        [ValidaAsignacionDeRoles] bit NULL,
        [ValidaEnrrolamiento] bit NULL,
        [RolBase] bit NULL,
        [Activo] bit NULL,
        [Nombre] nvarchar(256) NULL,
        [NombreNormalizado] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_Rol] PRIMARY KEY ([Id_Rol])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [UnidadOrganizacional] (
        [Id] uniqueidentifier NOT NULL,
        [Id_Organizacion] uniqueidentifier NOT NULL,
        [Codigo] nvarchar(100) NOT NULL,
        [Nombre] nvarchar(255) NOT NULL,
        [Descripcion] nvarchar(max) NOT NULL,
        [UnidadOrganizacionalBase] bit NOT NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_UnidadOrganizacional_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [Usuario] (
        [Id] nvarchar(450) NOT NULL,
        [IdPersona] nvarchar(max) NULL,
        [NombreADesplegar] nvarchar(max) NULL,
        [Descripcion] nvarchar(max) NULL,
        [TipoDeUsuario] nvarchar(max) NULL,
        [Activo] bit NULL,
        [UsuarioBase] bit NULL,
        [RequiereValidacionEnrrolamiento] bit NULL,
        [EstadoDeUsuario] nvarchar(max) NULL,
        [InformacionAdicional] nvarchar(max) NULL,
        [RefreshToken] nvarchar(max) NULL,
        [RefreshTokenExpiresAtUtc] datetimeoffset NULL,
        [NombreUsuario] nvarchar(256) NULL,
        [NombreUsuarioNormalizado] nvarchar(256) NULL,
        [CorreoElectronico] nvarchar(256) NULL,
        [CorreoElectronicoNormalizado] nvarchar(256) NULL,
        [CorreoElectronicoConfirmado] bit NOT NULL,
        [HashDeLaClave] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [NumeroDeTelefono] nvarchar(max) NULL,
        [NumeroDeTelefonoConfirmado] bit NOT NULL,
        [DobleFactorHabilitado] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [CantidadDeAccesosFallidos] int NOT NULL,
        CONSTRAINT [PK_Usuario] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [ValidacionEnrrolamiento] (
        [Id] uniqueidentifier NOT NULL,
        [IdValidado_Usuario] uniqueidentifier NOT NULL,
        [IdValidaEnrrolamiento_Usuario] uniqueidentifier NOT NULL,
        [EnrrolamientoAceptado] bit NOT NULL,
        [FechaValidacion] datetimeoffset NOT NULL,
        [FechaRegistro] datetimeoffset NOT NULL,
        [Activo] bit NOT NULL,
        CONSTRAINT [PK_ValidacionEnrrolamiento_Id] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_Rol_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Rol] ([Id_Rol]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] nvarchar(450) NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_Usuario_UserId] FOREIGN KEY ([UserId]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_Usuario_UserId] FOREIGN KEY ([UserId]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] nvarchar(450) NOT NULL,
        [RoleId] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_Rol_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Rol] ([Id_Rol]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_Usuario_UserId] FOREIGN KEY ([UserId]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] nvarchar(450) NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_Usuario_UserId] FOREIGN KEY ([UserId]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE TABLE [RefreshTokens] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] nvarchar(64) NOT NULL,
        [TokenHash] nvarchar(128) NOT NULL,
        [CreatedAtUtc] datetimeoffset NULL,
        [ExpiresAtUtc] datetimeoffset NULL,
        [RevokedAtUtc] datetimeoffset NULL,
        [SelectedOrganization] nvarchar(256) NULL,
        [ReplacedByTokenHash] nvarchar(128) NULL,
        [RevocationReason] nvarchar(64) NULL,
        [UserId] nvarchar(450) NOT NULL,
        [Id_Entidad] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_RefreshTokens] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_RefreshTokens_Usuario_UserId] FOREIGN KEY ([UserId]) REFERENCES [Usuario] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'AutenticadorExternoBase', N'ClaveDeAcceso', N'Id_Proveedor', N'Id_Usuario', N'NombreADesplegar', N'NombreUsuario', N'ValidadorPrimario') AND [object_id] = OBJECT_ID(N'[AutenticadorExterno]'))
        SET IDENTITY_INSERT [AutenticadorExterno] ON;
    EXEC(N'INSERT INTO [AutenticadorExterno] ([Id], [Activo], [AutenticadorExternoBase], [ClaveDeAcceso], [Id_Proveedor], [Id_Usuario], [NombreADesplegar], [NombreUsuario], [ValidadorPrimario])
    VALUES (''ecaf1074-722d-468f-81fa-69c2d7b88d68'', CAST(1 AS bit), CAST(1 AS bit), N''AQAAAAIAAYagAAAAEL23Xp7j+JxUsrKAljUnZ89wxmX/rYfRl8mrMVy20i8pAO1tng9S7zJYwpOucvcyrg=='', ''701c19bf-405c-4467-85f0-ddbc3786f9ee'', ''2b12d04f-c167-4ad1-a42a-e2ecd30518d7'', N''ADMINISTRADOR'', N''ADMINISTRADOR'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'AutenticadorExternoBase', N'ClaveDeAcceso', N'Id_Proveedor', N'Id_Usuario', N'NombreADesplegar', N'NombreUsuario', N'ValidadorPrimario') AND [object_id] = OBJECT_ID(N'[AutenticadorExterno]'))
        SET IDENTITY_INSERT [AutenticadorExterno] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CorreoElectronico', N'EntidadBase', N'FechaCreacion', N'FechaInicioAutorizacion', N'FechaTerminoAutorizacion', N'Id_UnidadOrganizacional', N'Id_Usuario', N'Principal', N'TipoDeEntidad') AND [object_id] = OBJECT_ID(N'[Entidad]'))
        SET IDENTITY_INSERT [Entidad] ON;
    EXEC(N'INSERT INTO [Entidad] ([Id], [CorreoElectronico], [EntidadBase], [FechaCreacion], [FechaInicioAutorizacion], [FechaTerminoAutorizacion], [Id_UnidadOrganizacional], [Id_Usuario], [Principal], [TipoDeEntidad])
    VALUES (''05507441-5792-4c46-9334-9a5faa99e20a'', N'''', CAST(1 AS bit), ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''198c164d-1cd8-4107-9db3-74b9fa33302c'', ''2b12d04f-c167-4ad1-a42a-e2ecd30518d7'', CAST(1 AS bit), N''PERSONA'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CorreoElectronico', N'EntidadBase', N'FechaCreacion', N'FechaInicioAutorizacion', N'FechaTerminoAutorizacion', N'Id_UnidadOrganizacional', N'Id_Usuario', N'Principal', N'TipoDeEntidad') AND [object_id] = OBJECT_ID(N'[Entidad]'))
        SET IDENTITY_INSERT [Entidad] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Codigo', N'Descripcion', N'IdOrganizacion', N'Nombre', N'OrganizacionBase') AND [object_id] = OBJECT_ID(N'[Organizacion]'))
        SET IDENTITY_INSERT [Organizacion] ON;
    EXEC(N'INSERT INTO [Organizacion] ([Id], [Activo], [Codigo], [Descripcion], [IdOrganizacion], [Nombre], [OrganizacionBase])
    VALUES (''70c699b4-eb37-49a4-9dcf-1fc87be16489'', CAST(1 AS bit), N''ANID'', N''Entidad pública encargada de promover la investigación y el desarrollo en Chile.'', N'''', N''Agencia Nacional de Investigación y Desarrollo'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Codigo', N'Descripcion', N'IdOrganizacion', N'Nombre', N'OrganizacionBase') AND [object_id] = OBJECT_ID(N'[Organizacion]'))
        SET IDENTITY_INSERT [Organizacion] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FechaCreacion', N'FechaInicioAsignacion', N'FechaTerminoAsignacion', N'Id_Entidad', N'Id_Proceso', N'Id_Rol', N'PoliticaAsignadaBase', N'RolAsignadoValidado', N'RolRequiereValidacion') AND [object_id] = OBJECT_ID(N'[PoliticaAsignada]'))
        SET IDENTITY_INSERT [PoliticaAsignada] ON;
    EXEC(N'INSERT INTO [PoliticaAsignada] ([Id], [FechaCreacion], [FechaInicioAsignacion], [FechaTerminoAsignacion], [Id_Entidad], [Id_Proceso], [Id_Rol], [PoliticaAsignadaBase], [RolAsignadoValidado], [RolRequiereValidacion])
    VALUES (''60838d42-c2df-402c-9253-ab3ce52ffb55'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''d1889c7c-c5dc-4d9a-a2fe-34cdf956b145'', ''856a08fd-4162-47cb-bf92-ff25029f3546'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb56'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb57'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''eec159f1-9ba9-463d-8407-ec2951751c29'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb58'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''4bb69d17-cc38-4e52-ad36-42226aa5723d'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb59'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''35e9a345-dda9-45fc-9a35-7cf27ec8e947'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb5a'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''06001a21-9b5f-47a3-ae8b-c749e531f9b1'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb5b'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''e256405c-0bda-479a-8a41-a043c672f9b1'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb5c'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''2b597b09-55ad-4304-b57d-76bd2df5ac4c'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb5d'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''0737dfc9-8c0f-44ed-ada9-0d227d6a4b5c'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb5e'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb5f'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''f2224186-ffcc-41ee-bbce-4bbd72504e22'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb60'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''f3285b94-90a4-4610-8c65-2782d2e3a1a3'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''60838d42-c2df-402c-9253-ab3ce52ffb61'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''e150709d-4fed-480b-8916-8b0837a775d3'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''8f589ba2-3bc0-40ea-b7c2-7aaaee278d6d'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''d1889c7c-c5dc-4d9a-a2fe-34cdf956b145'', ''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit)),
    (''bf05f4af-4bbc-472f-a7ed-bbd6d5d1af61'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''d1889c7c-c5dc-4d9a-a2fe-34cdf956b145'', ''03b6b706-a24f-4505-9ef6-e3ae7d48c907'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FechaCreacion', N'FechaInicioAsignacion', N'FechaTerminoAsignacion', N'Id_Entidad', N'Id_Proceso', N'Id_Rol', N'PoliticaAsignadaBase', N'RolAsignadoValidado', N'RolRequiereValidacion') AND [object_id] = OBJECT_ID(N'[PoliticaAsignada]'))
        SET IDENTITY_INSERT [PoliticaAsignada] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Codigo', N'ComoDesplegarUrlDeProceso', N'Contexto', N'Descripcion', N'IdMacro_Proceso', N'MaximaAsignacionDeRoles', N'NivelDeProceso', N'Nombre', N'ProcesoBase', N'Token', N'Url') AND [object_id] = OBJECT_ID(N'[Proceso]'))
        SET IDENTITY_INSERT [Proceso] ON;
    EXEC(N'INSERT INTO [Proceso] ([Id], [Activo], [Codigo], [ComoDesplegarUrlDeProceso], [Contexto], [Descripcion], [IdMacro_Proceso], [MaximaAsignacionDeRoles], [NivelDeProceso], [Nombre], [ProcesoBase], [Token], [Url])
    VALUES (''06001a21-9b5f-47a3-ae8b-c749e531f9b1'', CAST(1 AS bit), N''SEGUIMIENTO_FINANCIERO'', N''IFRAME'', N''Sistema de Seguimiento Financiero'', N''Sistema de Seguimiento Financiero'', ''00000000-0000-0000-0000-000000000000'', 1, N''NIVEL_MACRO'', N''Sistema de Seguimiento Financiero'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''0737dfc9-8c0f-44ed-ada9-0d227d6a4b5c'', CAST(1 AS bit), N''SFI_SISTEMA_TERMINO_SIA'', N''IFRAME'', N''Sistema Sistema Termino SIA'', N''Sistema Sistema Termino SIA'', ''06001a21-9b5f-47a3-ae8b-c749e531f9b1'', 1, N''NIVEL_SISTEMA'', N''Sistema Sistema Termino SIA'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''233783de-9094-4030-907d-82d7c5abf10e'', CAST(1 AS bit), N''POSTULACION'', N''IFRAME'', N''Sistema de Postulaciones'', N''Proceso de Postulacion'', ''00000000-0000-0000-0000-000000000000'', 1, N''NIVEL_MACRO'', N''Proceso de Postulacion'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''2b597b09-55ad-4304-b57d-76bd2df5ac4c'', CAST(1 AS bit), N''SFI_SISFON_LUTHIEN_SPI_SCH'', N''IFRAME'', N''Sistema Sisfon Luthien SPI SCH'', N''Sistema Sisfon Luthien SPI SCH'', ''06001a21-9b5f-47a3-ae8b-c749e531f9b1'', 1, N''NIVEL_SISTEMA'', N''Sistema Sisfon Luthien SPI SCH'', CAST(0 AS bit), N'''', N''172.16.4.107:22''),
    (''35e9a345-dda9-45fc-9a35-7cf27ec8e947'', CAST(1 AS bit), N''VIN_DATACIENCIA'', N''IFRAME'', N''Sistema DataCiencia'', N''Sistema DataCiencia'', ''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', 1, N''NIVEL_SISTEMA'', N''Sistema DataCiencia'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''3f5cceaf-4c86-4495-90b4-aafe7c4ab509'', CAST(1 AS bit), N''POS_ACONCAGUA_GENERICO'', N''REDIRECCION'', N''Sistema Aconcagua generico'', N''Sistema Aconcagua Generico'', ''233783de-9094-4030-907d-82d7c5abf10e'', 1, N''NIVEL_SISTEMA'', N''Sistema Aconcagua Generico'', CAST(0 AS bit), N'''', N''http://auth-qa05.anid.cl''),
    (''4bb69d17-cc38-4e52-ad36-42226aa5723d'', CAST(1 AS bit), N''VIN_BEIC'', N''IFRAME'', N''Sistema Beic'', N''Sistema Beic'', ''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', 1, N''NIVEL_SISTEMA'', N''Sistema Beic'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''587ec39b-e7d8-4c0f-9ac4-697940cf7b07'', CAST(1 AS bit), N''VIN_DIODI'', N''IFRAME'', N''Sistema DIODI'', N''Sistema DIODI'', ''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', 1, N''NIVEL_SISTEMA'', N''Sistema DIODI'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''5eeb676d-b3fc-4db7-9421-358a6c26d3dc'', CAST(1 AS bit), N''SFO_EVAL_GENERICO'', N''VENTANA'', N''Sistema Eval Generico'', N''Sistema Eval Generico'', ''c4a10de0-791c-45ec-820c-1a8802cd3e80'', 1, N''NIVEL_SISTEMA'', N''Sistema Eval Generico'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''6bd742e3-0e0a-4990-b51f-661c710ba4b9'', CAST(1 AS bit), N''SFO_EVAL_SPI'', N''VENTANA'', N''Sistema Eval SPI'', N''Sistema Eval SPI'', ''c4a10de0-791c-45ec-820c-1a8802cd3e80'', 1, N''NIVEL_SISTEMA'', N''Sistema Eval SPI'', CAST(0 AS bit), N'''', N''https://faraondesa.anid.cl/desa2/Evaluacion_TESTING/index.php''),
    (''7faa600f-2f47-4168-9a65-3c6d47ef2241'', CAST(1 AS bit), N''SFO_EVAL_BECAS'', N''VENTANA'', N''Sistema Eval Becas'', N''Sistema Eval Becas'', ''c4a10de0-791c-45ec-820c-1a8802cd3e80'', 1, N''NIVEL_SISTEMA'', N''Sistema Eval Becas'', CAST(0 AS bit), N'''', N''https://servicios-qa.anid.cl/evalbecas''),
    (''8ff51db1-44a5-4b9e-b0eb-0cd739cee604'', CAST(1 AS bit), N''STE_SISTEMA_VERDE_SIA'', N''IFRAME'', N''Sistema Verde SIA'', N''Sistema Verde SIA'', ''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', 1, N''NIVEL_SISTEMA'', N''Sistema Verde SIA'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''9679412f-ae9f-4d77-8629-7559d280ece2'', CAST(1 AS bit), N''SFO_FALLO_BECA'', N''IFRAME'', N''Sistema Fallo Beca'', N''Sistema Fallo Beca'', ''c4a10de0-791c-45ec-820c-1a8802cd3e80'', 1, N''NIVEL_SISTEMA'', N''Sistema Fallo Beca'', CAST(0 AS bit), N'''', N''https://servicios-qa.anid.cl/web//fallo/#/public''),
    (''9c4a10ea-d6f3-4a0c-b4b0-25fb0396c7ee'', CAST(1 AS bit), N''VIN_REPOSITORIO'', N''IFRAME'', N''Sistema Repositorio'', N''Sistema Repositorio'', ''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', 1, N''NIVEL_SISTEMA'', N''Sistema Repositorio'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''9cc3ac0d-503a-42b1-bc8e-10d20265e15b'', CAST(1 AS bit), N''STE_SISTEMA_TERMINO_SIA'', N''IFRAME'', N''Sistema Termino SIA'', N''Sistema Termino SIA'', ''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', 1, N''NIVEL_SISTEMA'', N''Sistema Termino SIA'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''a4ebe253-17c4-4a98-a333-d6fee919a212'', CAST(1 AS bit), N''VIN_PDI'', N''IFRAME'', N''Sistema PDI'', N''Sistema PDI'', ''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', 1, N''NIVEL_SISTEMA'', N''Sistema PDI'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''a6151eed-ea81-4edb-9c89-5bf7e260989f'', CAST(1 AS bit), N''SFO_FALLO_SPI'', N''IFRAME'', N''Sistema Fallo SPI'', N''Sistema Fallo SPI'', ''c4a10de0-791c-45ec-820c-1a8802cd3e80'', 1, N''NIVEL_SISTEMA'', N''Sistema Fallo SPI'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''a6ef77f1-e26b-430d-9d94-eef157e4df65'', CAST(1 AS bit), N''POS_MILENIO'', N''REDIRECCION'', N''Sistema de Postulaciones Milenio'', N''Proceso de Postulacion Milenio'', ''233783de-9094-4030-907d-82d7c5abf10e'', 1, N''NIVEL_SISTEMA'', N''Proceso de Postulacion Milenio'', CAST(0 AS bit), N'''', N''https://post-im.conicyt.cl/Concursos''),
    (''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', CAST(1 AS bit), N''SEGUIMIENTO_TECNICO'', N''IFRAME'', N''Sistema de Seguimiento Técnico'', N''Sistema de Seguimiento Técnico'', ''00000000-0000-0000-0000-000000000000'', 1, N''NIVEL_MACRO'', N''Sistema de Seguimiento Técnico'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''afc327c0-3970-40c7-9ef6-c0a3fdb42cb9'', CAST(1 AS bit), N''POS_ACONCAGUA_SPI'', N''REDIRECCION'', N''Sistema Aconcagua SPI'', N''Sistema Aconcagua SPI'', ''233783de-9094-4030-907d-82d7c5abf10e'', 1, N''NIVEL_SISTEMA'', N''Sistema Aconcagua SPI'', CAST(0 AS bit), N'''', N''https://auth-qa01.anid.cl''),
    (''c4a10de0-791c-45ec-820c-1a8802cd3e80'', CAST(1 AS bit), N''SELECCION_FORMALIZACION'', N''IFRAME'', N''Sistema de Seleccion y Formalizacion'', N''Sistema de Seleccion y Formalizacion'', ''00000000-0000-0000-0000-000000000000'', 1, N''NIVEL_MACRO'', N''Sistema de Seleccion y Formalizacion'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''c6b401ab-5164-43cc-8ae4-28ca2c9edbbe'', CAST(1 AS bit), N''POS_GENESIS'', N''REDIRECCION'', N''Sistema Genesis'', N''Sistema Genesis'', ''233783de-9094-4030-907d-82d7c5abf10e'', 1, N''NIVEL_SISTEMA'', N''Sistema Genesis'', CAST(0 AS bit), N'''', N''https://splqa.anid.cl''),
    (''d1889c7c-c5dc-4d9a-a2fe-34cdf956b145'', CAST(1 AS bit), N''ADMINISTRACION'', N''IFRAME'', N''Sistema de Administtracion de Permisos'', N''Manejo y asignacion de autorizaciones a los usuarios'', ''00000000-0000-0000-0000-000000000000'', 0, N''NIVEL_MACRO'', N''Sistema de Autorización'', CAST(1 AS bit), N'''', N''http://localhost:4210''),
    (''d65b2a0d-0742-491b-be4f-c7c00219821d'', CAST(1 AS bit), N''SFI_SYC_FINANCIERO_SIA'', N''IFRAME'', N''Sistema SyC Financiero SIA'', N''Sistema SyC Financiero SIA'', ''06001a21-9b5f-47a3-ae8b-c749e531f9b1'', 1, N''NIVEL_SISTEMA'', N''Sistema SyC Financiero SIA'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''e150709d-4fed-480b-8916-8b0837a775d3'', CAST(1 AS bit), N''STE_GESTION_MILENIO'', N''IFRAME'', N''Sistema Gestion Milenio'', N''Sistema Gestion Milenio'', ''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', 1, N''NIVEL_SISTEMA'', N''Sistema Gestion Milenio'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''e256405c-0bda-479a-8a41-a043c672f9b1'', CAST(1 AS bit), N''SFI_SGDL_SPI'', N''IFRAME'', N''Sistema SDGL SPI'', N''Sistema SDGL SPI'', ''06001a21-9b5f-47a3-ae8b-c749e531f9b1'', 1, N''NIVEL_SISTEMA'', N''Sistema SDGL SPI'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''eec159f1-9ba9-463d-8407-ec2951751c29'', CAST(1 AS bit), N''VIN_SCIELO'', N''IFRAME'', N''Sistema Scielo'', N''Sistema Scielo'', ''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', 1, N''NIVEL_SISTEMA'', N''Sistema Scielo'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''f2224186-ffcc-41ee-bbce-4bbd72504e22'', CAST(1 AS bit), N''STE_SIAL_SPI'', N''IFRAME'', N''Sistema SIAL SPI'', N''Sistema SIAL SPI'', ''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', 1, N''NIVEL_SISTEMA'', N''Sistema SIAL SPI'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''f3285b94-90a4-4610-8c65-2782d2e3a1a3'', CAST(1 AS bit), N''STE_SyC_LEGACY_SIA'', N''IFRAME'', N''Sistema SyC Legacy SIA'', N''Sistema SyC Legacy SIA'', ''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', 1, N''NIVEL_SISTEMA'', N''Sistema SyC Legacy SIA'', CAST(0 AS bit), N'''', N''http://localhost:4210''),
    (''f78ccd33-9762-4beb-83bc-7f2b02a295d7'', CAST(1 AS bit), N''SFO_FIRMA_CONVENIO'', N''IFRAME'', N''Sistema de Seleccion y Autorizacion Firma de Convenio'', N''Sistema de Seleccion y Autorizacion Firma de Convenio'', ''c4a10de0-791c-45ec-820c-1a8802cd3e80'', 1, N''NIVEL_SISTEMA'', N''Sistema de Seleccion y Autorizacion Firma de Convenio'', CAST(0 AS bit), N'''', N''https://servicios-qa.anid.cl/web/firma-convenio/#/login''),
    (''fa2e0bda-3063-4357-91b5-17f443b74ebd'', CAST(1 AS bit), N''STE_SISFON_LUTHIEN_SPI_SCH'', N''IFRAME'', N''Sistema Sisfon Luthien SPI SCH'', N''Sistema Sisfon Luthien SPI SCH'', ''aeaeb19b-2206-4870-9a99-f5d40a982b2e'', 1, N''NIVEL_SISTEMA'', N''Sistema Sisfon Luthien SPI SCH'', CAST(0 AS bit), N'''', N''172.16.4.107:22''),
    (''fcd2dcf8-7230-4fdd-9651-b4efeb60d11f'', CAST(1 AS bit), N''VIN_SCIELO'', N''IFRAME'', N''Sistema Scielo'', N''Sistema Scielo'', ''00000000-0000-0000-0000-000000000000'', 1, N''NIVEL_MACRO'', N''Sistema Scielo'', CAST(0 AS bit), N'''', N''http://localhost:4210'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Codigo', N'ComoDesplegarUrlDeProceso', N'Contexto', N'Descripcion', N'IdMacro_Proceso', N'MaximaAsignacionDeRoles', N'NivelDeProceso', N'Nombre', N'ProcesoBase', N'Token', N'Url') AND [object_id] = OBJECT_ID(N'[Proceso]'))
        SET IDENTITY_INSERT [Proceso] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'APIDeAutenticacion', N'Activo', N'Codigo', N'Descripcion', N'Nombre', N'ProveedorBase') AND [object_id] = OBJECT_ID(N'[Proveedor]'))
        SET IDENTITY_INSERT [Proveedor] ON;
    EXEC(N'INSERT INTO [Proveedor] ([Id], [APIDeAutenticacion], [Activo], [Codigo], [Descripcion], [Nombre], [ProveedorBase])
    VALUES (''701c19bf-405c-4467-85f0-ddbc3786f9ee'', N''https://www.aut2.darksidetech.services/'', CAST(1 AS bit), N''ANID'', N''Proveedor de autenticación implementadop or ANID.'', N''ANID'', CAST(1 AS bit)),
    (''9d9b40fa-fecb-41f6-ad4f-8ed7ceb1be13'', N''https://api.claveunica.gob.cl'', CAST(1 AS bit), N''CLAVEUNICA'', N''Proveedor de autenticación del gobierno chileno para servicios públicos.'', N''Clave Única'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'APIDeAutenticacion', N'Activo', N'Codigo', N'Descripcion', N'Nombre', N'ProveedorBase') AND [object_id] = OBJECT_ID(N'[Proveedor]'))
        SET IDENTITY_INSERT [Proveedor] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id_Rol', N'ActivaDetalleDeAutorizaciones', N'Activo', N'ConcurrencyStamp', N'Descripcion', N'Nombre', N'NombreNormalizado', N'RequiereAccionParaSerAsignado', N'RequiereValidacionDeAsignacion', N'RolBase', N'ValidaAsignacionDeRoles', N'ValidaEnrrolamiento') AND [object_id] = OBJECT_ID(N'[Rol]'))
        SET IDENTITY_INSERT [Rol] ON;
    EXEC(N'INSERT INTO [Rol] ([Id_Rol], [ActivaDetalleDeAutorizaciones], [Activo], [ConcurrencyStamp], [Descripcion], [Nombre], [NombreNormalizado], [RequiereAccionParaSerAsignado], [RequiereValidacionDeAsignacion], [RolBase], [ValidaAsignacionDeRoles], [ValidaEnrrolamiento])
    VALUES (N''03b6b706-a24f-4505-9ef6-e3ae7d48c907'', CAST(0 AS bit), CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d002004'', N'''', N''VALIDA_ASIGNACION_ROLES'', N''VALIDA_ASIGNACION_ROLES'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit)),
    (N''36957ec2-2857-4101-a81b-f0340bf8eff2'', CAST(0 AS bit), CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d002002'', N'''', N''ADMINISTRADOR_ENTIDAD'', N''ADMINISTRADOR_ENTIDAD'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit)),
    (N''856a08fd-4162-47cb-bf92-ff25029f3546'', CAST(0 AS bit), CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d002005'', N'''', N''VALIDA_ENRROLAMIENTO'', N''VALIDA_ENRROLAMIENTO'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit)),
    (N''c42e4c85-2b6d-4c8f-8672-f82a2d1c2d9e'', CAST(0 AS bit), CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d002001'', N'''', N''ADMINISTRADOR'', N''ADMINISTRADOR'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit)),
    (N''e198ec28-2b1b-48b0-8d5e-eb946d596e90'', CAST(0 AS bit), CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d002006'', N'''', N''USUARIO'', N''USUARIO'', CAST(0 AS bit), CAST(0 AS bit), CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit)),
    (N''e3093727-b36b-45af-a495-7c3d0804c0e9'', CAST(0 AS bit), CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d002003'', N'''', N''ADMINISTRADOR_UNIDAD'', N''ADMINISTRADOR_UNIDAD'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id_Rol', N'ActivaDetalleDeAutorizaciones', N'Activo', N'ConcurrencyStamp', N'Descripcion', N'Nombre', N'NombreNormalizado', N'RequiereAccionParaSerAsignado', N'RequiereValidacionDeAsignacion', N'RolBase', N'ValidaAsignacionDeRoles', N'ValidaEnrrolamiento') AND [object_id] = OBJECT_ID(N'[Rol]'))
        SET IDENTITY_INSERT [Rol] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Codigo', N'Descripcion', N'Id_Organizacion', N'Nombre', N'UnidadOrganizacionalBase') AND [object_id] = OBJECT_ID(N'[UnidadOrganizacional]'))
        SET IDENTITY_INSERT [UnidadOrganizacional] ON;
    EXEC(N'INSERT INTO [UnidadOrganizacional] ([Id], [Activo], [Codigo], [Descripcion], [Id_Organizacion], [Nombre], [UnidadOrganizacionalBase])
    VALUES (''198c164d-1cd8-4107-9db3-74b9fa33302c'', CAST(1 AS bit), N''CASA_MATRIZ'', N''Casa Matriz de la Agencia Nacional de Investigación y Desarrollo'', ''70c699b4-eb37-49a4-9dcf-1fc87be16489'', N''Casa Matriz ANID'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Activo', N'Codigo', N'Descripcion', N'Id_Organizacion', N'Nombre', N'UnidadOrganizacionalBase') AND [object_id] = OBJECT_ID(N'[UnidadOrganizacional]'))
        SET IDENTITY_INSERT [UnidadOrganizacional] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CantidadDeAccesosFallidos', N'Activo', N'ConcurrencyStamp', N'Descripcion', N'CorreoElectronico', N'CorreoElectronicoConfirmado', N'EstadoDeUsuario', N'IdPersona', N'InformacionAdicional', N'LockoutEnabled', N'LockoutEnd', N'NombreADesplegar', N'CorreoElectronicoNormalizado', N'NombreUsuarioNormalizado', N'HashDeLaClave', N'NumeroDeTelefono', N'NumeroDeTelefonoConfirmado', N'RefreshToken', N'RefreshTokenExpiresAtUtc', N'RequiereValidacionEnrrolamiento', N'SecurityStamp', N'TipoDeUsuario', N'DobleFactorHabilitado', N'NombreUsuario', N'UsuarioBase') AND [object_id] = OBJECT_ID(N'[Usuario]'))
        SET IDENTITY_INSERT [Usuario] ON;
    EXEC(N'INSERT INTO [Usuario] ([Id], [CantidadDeAccesosFallidos], [Activo], [ConcurrencyStamp], [Descripcion], [CorreoElectronico], [CorreoElectronicoConfirmado], [EstadoDeUsuario], [IdPersona], [InformacionAdicional], [LockoutEnabled], [LockoutEnd], [NombreADesplegar], [CorreoElectronicoNormalizado], [NombreUsuarioNormalizado], [HashDeLaClave], [NumeroDeTelefono], [NumeroDeTelefonoConfirmado], [RefreshToken], [RefreshTokenExpiresAtUtc], [RequiereValidacionEnrrolamiento], [SecurityStamp], [TipoDeUsuario], [DobleFactorHabilitado], [NombreUsuario], [UsuarioBase])
    VALUES (N''2b12d04f-c167-4ad1-a42a-e2ecd30518d7'', 10, CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d001001'', N''Administrador global'', N''administrador@security.com'', CAST(1 AS bit), N''REGISTRADO'', N'''', N'''', CAST(0 AS bit), NULL, N''Administrador'', N''ADMINISTRADOR@SECURITY.COM'', N''ADMINISTRADOR'', N''AQAAAAIAAYagAAAAEL23Xp7j+JxUsrKAljUnZ89wxmX/rYfRl8mrMVy20i8pAO1tng9S7zJYwpOucvcyrg=='', N'''', CAST(1 AS bit), NULL, NULL, CAST(0 AS bit), N'''', N''NACIONAL'', CAST(0 AS bit), N''ADMINISTRADOR'', CAST(1 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'CantidadDeAccesosFallidos', N'Activo', N'ConcurrencyStamp', N'Descripcion', N'CorreoElectronico', N'CorreoElectronicoConfirmado', N'EstadoDeUsuario', N'IdPersona', N'InformacionAdicional', N'LockoutEnabled', N'LockoutEnd', N'NombreADesplegar', N'CorreoElectronicoNormalizado', N'NombreUsuarioNormalizado', N'HashDeLaClave', N'NumeroDeTelefono', N'NumeroDeTelefonoConfirmado', N'RefreshToken', N'RefreshTokenExpiresAtUtc', N'RequiereValidacionEnrrolamiento', N'SecurityStamp', N'TipoDeUsuario', N'DobleFactorHabilitado', N'NombreUsuario', N'UsuarioBase') AND [object_id] = OBJECT_ID(N'[Usuario]'))
        SET IDENTITY_INSERT [Usuario] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE INDEX [IX_RefreshTokens_SessionId] ON [RefreshTokens] ([SessionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RefreshTokens_TokenHash] ON [RefreshTokens] ([TokenHash]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE INDEX [IX_RefreshTokens_UserId] ON [RefreshTokens] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [Rol] ([NombreNormalizado]) WHERE [NombreNormalizado] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [Usuario] ([CorreoElectronicoNormalizado]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [Usuario] ([NombreUsuarioNormalizado]) WHERE [NombreUsuarioNormalizado] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260408205144_Inicial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260408205144_Inicial', N'10.0.7');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE TABLE [AuditAggregateCursor] (
        [AggregateId] uniqueidentifier NOT NULL,
        [LastRevision] bigint NOT NULL,
        [UpdatedAtUtc] datetimeoffset NOT NULL,
        CONSTRAINT [PK_AuditAggregateCursor] PRIMARY KEY ([AggregateId])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE TABLE [AuditOutbox] (
        [Id] uniqueidentifier NOT NULL,
        [CorrelationId] uniqueidentifier NOT NULL,
        [AggregateId] uniqueidentifier NOT NULL,
        [AggregateType] nvarchar(150) NOT NULL,
        [AggregateRevision] bigint NOT NULL,
        [EventType] nvarchar(200) NOT NULL,
        [CommandType] nvarchar(200) NOT NULL,
        [OperationType] int NOT NULL,
        [ActorUserId] nvarchar(100) NULL,
        [ActorUsername] nvarchar(256) NULL,
        [ActorEmail] nvarchar(256) NULL,
        [RequestPath] nvarchar(512) NULL,
        [OccurredAtUtc] datetimeoffset NOT NULL,
        [PersistedAtUtc] datetimeoffset NOT NULL,
        [SnapshotJson] nvarchar(max) NULL,
        [DispatchStatus] smallint NOT NULL DEFAULT CAST(0 AS smallint),
        [DispatchAttempts] int NOT NULL DEFAULT 0,
        [LastDispatchAttemptUtc] datetimeoffset NULL,
        [DispatchedAtUtc] datetimeoffset NULL,
        [LastError] nvarchar(max) NULL,
        [SchemaVersion] smallint NOT NULL DEFAULT CAST(1 AS smallint),
        CONSTRAINT [PK_AuditOutbox] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE TABLE [AuditOutboxChange] (
        [Id] uniqueidentifier NOT NULL,
        [AuditOutboxMessageId] uniqueidentifier NOT NULL,
        [Order] int NOT NULL,
        [Path] nvarchar(200) NOT NULL,
        [ValueType] nvarchar(512) NULL,
        [NewValueJson] nvarchar(max) NULL,
        CONSTRAINT [PK_AuditOutboxChange] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AuditOutboxChange_AuditOutbox_AuditOutboxMessageId] FOREIGN KEY ([AuditOutboxMessageId]) REFERENCES [AuditOutbox] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE INDEX [IX_AuditOutbox_ActorUserId_OccurredAtUtc] ON [AuditOutbox] ([ActorUserId], [OccurredAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE UNIQUE INDEX [IX_AuditOutbox_AggregateId_AggregateRevision] ON [AuditOutbox] ([AggregateId], [AggregateRevision]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE INDEX [IX_AuditOutbox_AggregateId_OccurredAtUtc_Id] ON [AuditOutbox] ([AggregateId], [OccurredAtUtc], [Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE INDEX [IX_AuditOutbox_CorrelationId] ON [AuditOutbox] ([CorrelationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE INDEX [IX_AuditOutbox_DispatchStatus_PersistedAtUtc_Id] ON [AuditOutbox] ([DispatchStatus], [PersistedAtUtc], [Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE INDEX [IX_AuditOutbox_EventType_OccurredAtUtc] ON [AuditOutbox] ([EventType], [OccurredAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE INDEX [IX_AuditOutboxChange_AuditOutboxMessageId] ON [AuditOutboxChange] ([AuditOutboxMessageId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    CREATE INDEX [IX_AuditOutboxChange_Path] ON [AuditOutboxChange] ([Path]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260411163556_AddAuditTraceability'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260411163556_AddAuditTraceability', N'10.0.7');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260412004336_AddAuditAggregateCursorConcurrencyToken'
)
BEGIN
    ALTER TABLE [AuditAggregateCursor] ADD [ConcurrencyToken] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260412004336_AddAuditAggregateCursorConcurrencyToken'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FechaCreacion', N'FechaInicioAsignacion', N'FechaTerminoAsignacion', N'Id_Entidad', N'Id_Proceso', N'Id_Rol', N'PoliticaAsignadaBase', N'RolAsignadoValidado', N'RolRequiereValidacion') AND [object_id] = OBJECT_ID(N'[PoliticaAsignada]'))
        SET IDENTITY_INSERT [PoliticaAsignada] ON;
    EXEC(N'INSERT INTO [PoliticaAsignada] ([Id], [FechaCreacion], [FechaInicioAsignacion], [FechaTerminoAsignacion], [Id_Entidad], [Id_Proceso], [Id_Rol], [PoliticaAsignadaBase], [RolAsignadoValidado], [RolRequiereValidacion])
    VALUES (''60838d42-c2df-402c-9253-ab3ce52ffb62'', ''2026-04-07T23:04:30.0000000+00:00'', NULL, NULL, ''05507441-5792-4c46-9334-9a5faa99e20a'', ''d1889c7c-c5dc-4d9a-a2fe-34cdf956b145'', ''e198ec28-2b1b-48b0-8d5e-eb946d596e91'', CAST(1 AS bit), CAST(1 AS bit), CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'FechaCreacion', N'FechaInicioAsignacion', N'FechaTerminoAsignacion', N'Id_Entidad', N'Id_Proceso', N'Id_Rol', N'PoliticaAsignadaBase', N'RolAsignadoValidado', N'RolRequiereValidacion') AND [object_id] = OBJECT_ID(N'[PoliticaAsignada]'))
        SET IDENTITY_INSERT [PoliticaAsignada] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260412004336_AddAuditAggregateCursorConcurrencyToken'
)
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id_Rol', N'ActivaDetalleDeAutorizaciones', N'Activo', N'ConcurrencyStamp', N'Descripcion', N'Nombre', N'NombreNormalizado', N'RequiereAccionParaSerAsignado', N'RequiereValidacionDeAsignacion', N'RolBase', N'ValidaAsignacionDeRoles', N'ValidaEnrrolamiento') AND [object_id] = OBJECT_ID(N'[Rol]'))
        SET IDENTITY_INSERT [Rol] ON;
    EXEC(N'INSERT INTO [Rol] ([Id_Rol], [ActivaDetalleDeAutorizaciones], [Activo], [ConcurrencyStamp], [Descripcion], [Nombre], [NombreNormalizado], [RequiereAccionParaSerAsignado], [RequiereValidacionDeAsignacion], [RolBase], [ValidaAsignacionDeRoles], [ValidaEnrrolamiento])
    VALUES (N''e198ec28-2b1b-48b0-8d5e-eb946d596e91'', CAST(0 AS bit), CAST(1 AS bit), N''4f4d7775-2677-47e7-8ccd-a34f5d002007'', N'''', N''AUDITOR_TRAZABILIDAD'', N''AUDITOR_TRAZABILIDAD'', CAST(1 AS bit), CAST(0 AS bit), CAST(1 AS bit), CAST(0 AS bit), CAST(0 AS bit))');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id_Rol', N'ActivaDetalleDeAutorizaciones', N'Activo', N'ConcurrencyStamp', N'Descripcion', N'Nombre', N'NombreNormalizado', N'RequiereAccionParaSerAsignado', N'RequiereValidacionDeAsignacion', N'RolBase', N'ValidaAsignacionDeRoles', N'ValidaEnrrolamiento') AND [object_id] = OBJECT_ID(N'[Rol]'))
        SET IDENTITY_INSERT [Rol] OFF;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260412004336_AddAuditAggregateCursorConcurrencyToken'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260412004336_AddAuditAggregateCursorConcurrencyToken', N'10.0.7');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260414014343_AddNotificationOutbox'
)
BEGIN
    CREATE TABLE [NotificationOutbox] (
        [Id] uniqueidentifier NOT NULL,
        [Channel] nvarchar(50) NOT NULL,
        [NotificationType] nvarchar(150) NOT NULL,
        [UserId] nvarchar(100) NULL,
        [RequestPath] nvarchar(512) NULL,
        [DeduplicationKey] nvarchar(300) NULL,
        [PayloadJson] nvarchar(max) NOT NULL,
        [DispatchStatus] smallint NOT NULL DEFAULT CAST(0 AS smallint),
        [DispatchAttempts] int NOT NULL DEFAULT 0,
        [CreatedAtUtc] datetimeoffset NOT NULL,
        [NextAttemptUtc] datetimeoffset NOT NULL,
        [LastDispatchAttemptUtc] datetimeoffset NULL,
        [DispatchedAtUtc] datetimeoffset NULL,
        [LastError] nvarchar(max) NULL,
        [SchemaVersion] smallint NOT NULL DEFAULT CAST(1 AS smallint),
        CONSTRAINT [PK_NotificationOutbox] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260414014343_AddNotificationOutbox'
)
BEGIN
    CREATE INDEX [IX_NotificationOutbox_Channel_NotificationType_CreatedAtUtc] ON [NotificationOutbox] ([Channel], [NotificationType], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260414014343_AddNotificationOutbox'
)
BEGIN
    CREATE INDEX [IX_NotificationOutbox_DeduplicationKey] ON [NotificationOutbox] ([DeduplicationKey]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260414014343_AddNotificationOutbox'
)
BEGIN
    CREATE INDEX [IX_NotificationOutbox_DispatchStatus_NextAttemptUtc_CreatedAtUtc_Id] ON [NotificationOutbox] ([DispatchStatus], [NextAttemptUtc], [CreatedAtUtc], [Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260414014343_AddNotificationOutbox'
)
BEGIN
    CREATE INDEX [IX_NotificationOutbox_UserId] ON [NotificationOutbox] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260414014343_AddNotificationOutbox'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260414014343_AddNotificationOutbox', N'10.0.7');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260416223845_AddPasswordChangeChallenges'
)
BEGIN
    CREATE TABLE [PasswordChangeChallenges] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] nvarchar(100) NOT NULL,
        [CodeHash] nvarchar(256) NOT NULL,
        [RequestPath] nvarchar(512) NULL,
        [FailedAttempts] int NOT NULL DEFAULT 0,
        [ResendCount] int NOT NULL DEFAULT 0,
        [CreatedAtUtc] datetimeoffset NOT NULL,
        [LastSentAtUtc] datetimeoffset NOT NULL,
        [ExpiresAtUtc] datetimeoffset NOT NULL,
        [ConsumedAtUtc] datetimeoffset NULL,
        [CancelledAtUtc] datetimeoffset NULL,
        CONSTRAINT [PK_PasswordChangeChallenges] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260416223845_AddPasswordChangeChallenges'
)
BEGIN
    CREATE INDEX [IX_PasswordChangeChallenges_UserId_CreatedAtUtc] ON [PasswordChangeChallenges] ([UserId], [CreatedAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260416223845_AddPasswordChangeChallenges'
)
BEGIN
    CREATE INDEX [IX_PasswordChangeChallenges_UserId_ExpiresAtUtc_ConsumedAtUtc_CancelledAtUtc] ON [PasswordChangeChallenges] ([UserId], [ExpiresAtUtc], [ConsumedAtUtc], [CancelledAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260416223845_AddPasswordChangeChallenges'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260416223845_AddPasswordChangeChallenges', N'10.0.7');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424120001_AddPasswordRecoveryChallengePurpose'
)
BEGIN
    DROP INDEX [IX_PasswordChangeChallenges_UserId_ExpiresAtUtc_ConsumedAtUtc_CancelledAtUtc] ON [PasswordChangeChallenges];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424120001_AddPasswordRecoveryChallengePurpose'
)
BEGIN
    ALTER TABLE [PasswordChangeChallenges] ADD [ChallengePurpose] nvarchar(50) NOT NULL DEFAULT N'PASSWORD_CHANGE';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424120001_AddPasswordRecoveryChallengePurpose'
)
BEGIN
    CREATE INDEX [IX_PasswordChangeChallenges_UserId_Purpose_Expires] ON [PasswordChangeChallenges] ([UserId], [ChallengePurpose], [ExpiresAtUtc], [ConsumedAtUtc], [CancelledAtUtc]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424120001_AddPasswordRecoveryChallengePurpose'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260424120001_AddPasswordRecoveryChallengePurpose', N'10.0.7');
END;

COMMIT;
GO

