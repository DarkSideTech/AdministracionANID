DO $QA$
DECLARE
    v_password_hash text := 'AQAAAAIAAYagAAAAEL23Xp7j+JxUsrKAljUnZ89wxmX/rYfRl8mrMVy20i8pAO1tng9S7zJYwpOucvcyrg==';
    v_now timestamp with time zone := TIMESTAMPTZ '2026-05-05T00:00:00+00:00';
    v_user record;
    v_role_id text;
    v_process record;
    v_process_count integer;
    v_policy_seed text;
    v_policy_id uuid;
BEGIN
    FOR v_user IN
        SELECT *
        FROM (VALUES
            ('f1000000-0000-4000-8000-000000000001', 'USR1QA', 'usr1qa@qa.local', 'Usuario QA Administrador', 'ADMINISTRADOR', 'f2000000-0000-4000-8000-000000000001'::uuid, 'QA_ORG_USR1', 'Organizacion QA USR1', 'f3000000-0000-4000-8000-000000000001'::uuid, 'QA_UO_USR1', 'Unidad Organizacional QA USR1', 'f4000000-0000-4000-8000-000000000001'::uuid),
            ('f1000000-0000-4000-8000-000000000002', 'USR2QA', 'usr2qa@qa.local', 'Usuario QA Administrador Entidad', 'ADMINISTRADOR_ENTIDAD', 'f2000000-0000-4000-8000-000000000002'::uuid, 'QA_ORG_USR2', 'Organizacion QA USR2', 'f3000000-0000-4000-8000-000000000002'::uuid, 'QA_UO_USR2', 'Unidad Organizacional QA USR2', 'f4000000-0000-4000-8000-000000000002'::uuid),
            ('f1000000-0000-4000-8000-000000000003', 'USR3QA', 'usr3qa@qa.local', 'Usuario QA Administrador Unidad', 'ADMINISTRADOR_UNIDAD', 'f2000000-0000-4000-8000-000000000003'::uuid, 'QA_ORG_USR3', 'Organizacion QA USR3', 'f3000000-0000-4000-8000-000000000003'::uuid, 'QA_UO_USR3', 'Unidad Organizacional QA USR3', 'f4000000-0000-4000-8000-000000000003'::uuid),
            ('f1000000-0000-4000-8000-000000000004', 'USR4QA', 'usr4qa@qa.local', 'Usuario QA Valida Asignacion Roles', 'VALIDA_ASIGNACION_ROLES', 'f2000000-0000-4000-8000-000000000004'::uuid, 'QA_ORG_USR4', 'Organizacion QA USR4', 'f3000000-0000-4000-8000-000000000004'::uuid, 'QA_UO_USR4', 'Unidad Organizacional QA USR4', 'f4000000-0000-4000-8000-000000000004'::uuid),
            ('f1000000-0000-4000-8000-000000000005', 'USR5QA', 'usr5qa@qa.local', 'Usuario QA Valida Enrrolamiento', 'VALIDA_ENRROLAMIENTO', 'f2000000-0000-4000-8000-000000000005'::uuid, 'QA_ORG_USR5', 'Organizacion QA USR5', 'f3000000-0000-4000-8000-000000000005'::uuid, 'QA_UO_USR5', 'Unidad Organizacional QA USR5', 'f4000000-0000-4000-8000-000000000005'::uuid),
            ('f1000000-0000-4000-8000-000000000006', 'USR6QA', 'usr6qa@qa.local', 'Usuario QA Auditor Trazabilidad', 'AUDITOR_TRAZABILIDAD', 'f2000000-0000-4000-8000-000000000006'::uuid, 'QA_ORG_USR6', 'Organizacion QA USR6', 'f3000000-0000-4000-8000-000000000006'::uuid, 'QA_UO_USR6', 'Unidad Organizacional QA USR6', 'f4000000-0000-4000-8000-000000000006'::uuid)
        ) AS seed(
            user_id,
            user_name,
            email,
            display_name,
            role_name,
            org_id,
            org_code,
            org_name,
            unit_id,
            unit_code,
            unit_name,
            entity_id
        )
    LOOP
        SELECT r."Id_Rol"
        INTO v_role_id
        FROM "Rol" r
        WHERE r."NombreNormalizado" = v_user.role_name
        LIMIT 1;

        IF v_role_id IS NULL THEN
            RAISE EXCEPTION 'SeedDataQa requiere el rol %, pero no existe en Rol.NombreNormalizado.', v_user.role_name;
        END IF;

        IF EXISTS (
            SELECT 1
            FROM "Usuario" u
            WHERE u."NombreUsuarioNormalizado" = upper(v_user.user_name)
              AND u."Id" <> v_user.user_id
        ) THEN
            RAISE EXCEPTION 'SeedDataQa no puede crear %, ya existe otro usuario con ese nombre normalizado.', v_user.user_name;
        END IF;

        INSERT INTO "Usuario" (
            "Id",
            "CantidadDeAccesosFallidos",
            "Activo",
            "ConcurrencyStamp",
            "Descripcion",
            "CorreoElectronico",
            "CorreoElectronicoConfirmado",
            "EstadoDeUsuario",
            "IdPersona",
            "InformacionAdicional",
            "LockoutEnabled",
            "LockoutEnd",
            "NombreADesplegar",
            "CorreoElectronicoNormalizado",
            "NombreUsuarioNormalizado",
            "HashDeLaClave",
            "NumeroDeTelefono",
            "NumeroDeTelefonoConfirmado",
            "RefreshToken",
            "RefreshTokenExpiresAtUtc",
            "RequiereValidacionEnrrolamiento",
            "SecurityStamp",
            "TipoDeUsuario",
            "DobleFactorHabilitado",
            "NombreUsuario",
            "UsuarioBase"
        )
        SELECT
            v_user.user_id,
            0,
            TRUE,
            md5(v_user.user_id || ':concurrency'),
            'Usuario QA para rol ' || v_user.role_name,
            v_user.email,
            TRUE,
            'REGISTRADO',
            '',
            '',
            FALSE,
            NULL,
            v_user.display_name,
            upper(v_user.email),
            upper(v_user.user_name),
            v_password_hash,
            '',
            FALSE,
            NULL,
            NULL,
            FALSE,
            md5(v_user.user_id || ':security'),
            'NACIONAL',
            FALSE,
            v_user.user_name,
            FALSE
        WHERE NOT EXISTS (
            SELECT 1
            FROM "Usuario" u
            WHERE u."Id" = v_user.user_id
        );

        INSERT INTO "AspNetUserRoles" ("UserId", "RoleId")
        SELECT v_user.user_id, v_role_id
        WHERE NOT EXISTS (
            SELECT 1
            FROM "AspNetUserRoles" ur
            WHERE ur."UserId" = v_user.user_id
              AND ur."RoleId" = v_role_id
        );

        INSERT INTO "Organizacion" (
            "Id",
            "Activo",
            "Codigo",
            "Descripcion",
            "IdOrganizacion",
            "Nombre",
            "OrganizacionBase"
        )
        SELECT
            v_user.org_id,
            TRUE,
            v_user.org_code,
            'Organizacion QA asociada a ' || v_user.user_name,
            v_user.org_code,
            v_user.org_name,
            FALSE
        WHERE NOT EXISTS (
            SELECT 1
            FROM "Organizacion" o
            WHERE o."Id" = v_user.org_id
        );

        INSERT INTO "UnidadOrganizacional" (
            "Id",
            "Activo",
            "Codigo",
            "Descripcion",
            "Id_Organizacion",
            "Nombre",
            "UnidadOrganizacionalBase"
        )
        SELECT
            v_user.unit_id,
            TRUE,
            v_user.unit_code,
            'Unidad organizacional QA asociada a ' || v_user.user_name,
            v_user.org_id,
            v_user.unit_name,
            FALSE
        WHERE NOT EXISTS (
            SELECT 1
            FROM "UnidadOrganizacional" uo
            WHERE uo."Id" = v_user.unit_id
        );

        INSERT INTO "Entidad" (
            "Id",
            "CorreoElectronico",
            "EntidadBase",
            "FechaCreacion",
            "FechaInicioAutorizacion",
            "FechaTerminoAutorizacion",
            "Id_UnidadOrganizacional",
            "Id_Usuario",
            "Principal",
            "TipoDeEntidad"
        )
        SELECT
            v_user.entity_id,
            v_user.email,
            FALSE,
            v_now,
            NULL,
            NULL,
            v_user.unit_id,
            v_user.user_id::uuid,
            TRUE,
            'PERSONA'
        WHERE NOT EXISTS (
            SELECT 1
            FROM "Entidad" e
            WHERE e."Id" = v_user.entity_id
        );

        v_process_count := 0;

        FOR v_process IN
            WITH system_processes AS (
                SELECT count(*) AS total
                FROM "Proceso" p
                WHERE p."Activo" IS TRUE
                  AND p."NivelDeProceso" = 'NIVEL_SISTEMA'
            ),
            candidates AS (
                SELECT
                    p."Id" AS process_id,
                    row_number() OVER (ORDER BY md5(p."Id"::text || ':' || v_user.user_name)) AS rn
                FROM "Proceso" p
                CROSS JOIN system_processes sp
                WHERE p."Activo" IS TRUE
                  AND (
                      (sp.total > 0 AND p."NivelDeProceso" = 'NIVEL_SISTEMA')
                      OR sp.total = 0
                  )
            )
            SELECT c.process_id
            FROM candidates c
            WHERE c.rn <= 4
        LOOP
            v_policy_seed := md5(v_user.entity_id::text || ':' || v_role_id || ':' || v_process.process_id::text);
            v_policy_id := (
                substr(v_policy_seed, 1, 8) || '-' ||
                substr(v_policy_seed, 9, 4) || '-4' ||
                substr(v_policy_seed, 14, 3) || '-8' ||
                substr(v_policy_seed, 18, 3) || '-' ||
                substr(v_policy_seed, 21, 12)
            )::uuid;

            INSERT INTO "PoliticaAsignada" (
                "Id",
                "FechaCreacion",
                "FechaInicioAsignacion",
                "FechaTerminoAsignacion",
                "Id_Entidad",
                "Id_Proceso",
                "Id_Rol",
                "PoliticaAsignadaBase",
                "RolAsignadoValidado",
                "RolRequiereValidacion"
            )
            SELECT
                v_policy_id,
                v_now,
                v_now,
                NULL,
                v_user.entity_id,
                v_process.process_id,
                v_role_id::uuid,
                FALSE,
                TRUE,
                FALSE
            WHERE NOT EXISTS (
                SELECT 1
                FROM "PoliticaAsignada" pa
                WHERE pa."Id_Entidad" = v_user.entity_id
                  AND pa."Id_Proceso" = v_process.process_id
                  AND pa."Id_Rol" = v_role_id::uuid
            )
            AND NOT EXISTS (
                SELECT 1
                FROM "PoliticaAsignada" pa
                WHERE pa."Id" = v_policy_id
            );

            v_process_count := v_process_count + 1;
        END LOOP;

        IF v_process_count = 0 THEN
            RAISE NOTICE 'SeedDataQa no encontro procesos activos para asignar al usuario %.', v_user.user_name;
        END IF;
    END LOOP;
END
$QA$;
