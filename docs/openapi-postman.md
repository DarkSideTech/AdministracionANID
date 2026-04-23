# OpenAPI y Postman

## Objetivo
La solucion queda con:

- `Microsoft.AspNetCore.OpenApi` como fuente integrada del documento OpenAPI.
- `Swagger UI` como interfaz temporal de exploracion manual en entorno local.
- `Postman` como herramienta principal de validacion operativa para flujos con CSRF, cookies, login y refresh token.

## Endpoints locales
- Swagger UI: `http://localhost:5002/swagger`
- OpenAPI JSON: `http://localhost:5002/openapi/v1.json`

## Postman
Archivos base:

- Coleccion: [AUT2Services.local.postman_collection.json](E:/DST/dev/AUT2Services/docs/postman/AUT2Services.local.postman_collection.json)
- Environment: [AUT2Services.local.postman_environment.json](E:/DST/dev/AUT2Services/docs/postman/AUT2Services.local.postman_environment.json)

## Preparacion por provider
Antes de ejecutar la coleccion, levantar la API con uno de estos flujos:

### PostgreSQL
```powershell
.\scripts\update-db-postgresql.ps1
.\scripts\run-postgresql.ps1
```

### SQL Server
```powershell
.\scripts\update-db-sqlserver.ps1
.\scripts\run-sqlserver.ps1
```

Referencia completa: [guia-ejecucion-por-provider.md](E:/DST/dev/AUT2Services/docs/guia-ejecucion-por-provider.md)

### Flujo recomendado
1. Importar la coleccion y el environment local.
2. Seleccionar el environment `AUT2Services Local`.
3. Confirmar que la API esta arriba en `http://localhost:5002/swagger`.
4. Ejecutar la carpeta `Auth Flow` completa en orden.

### Que automatiza la coleccion
- `CSRF Bootstrap`
  - llama `GET /api/account/csrf`
  - guarda la cookie `XSRF-TOKEN` en la variable `csrfToken`
- `Login`
  - envia `X-CSRF-TOKEN` con el valor de `csrfToken`
  - usa el mismo cookie jar de Postman
  - actualiza `csrfToken` con la cookie rotada que devuelve el backend despues del login
  - guarda errores en `loginError`
  - intenta extraer `AccessTokenExpiracion` y `SeleccionOrganizacionRequerida`
- `Refresh Token`
  - reutiliza la cookie jar del login
  - envia `X-CSRF-TOKEN` usando el valor mas reciente de la cookie `XSRF-TOKEN`
  - guarda errores en `refreshError`

## Variables relevantes
- `baseUrl`
  - por defecto: `http://localhost:5002`
- `email`
  - por defecto: `administrador@security.com`
- `password`
  - por defecto: `Changeme123#`
- `csrfToken`
  - se completa automaticamente desde la cookie `XSRF-TOKEN`
- `accessTokenExpiracion`
  - se completa automaticamente desde la respuesta del backend
- `seleccionOrganizacionRequerida`
  - se completa automaticamente desde la respuesta del backend
- `loginError`
  - se completa automaticamente si `Login` no responde `200`
- `refreshError`
  - se completa automaticamente si `Refresh Token` no responde `200`

## Resultado esperado
Con la base local alineada y la API levantada por script:

- `CSRF Bootstrap`: `204`
- `Login`: `200`
- `Refresh Token`: `200`

Estado verificado:
- PostgreSQL local: OK
- SQL Server local en Docker: OK

Referencia: [estado-actual-local-2026-04-08.md](E:/DST/dev/AUT2Services/docs/estado-actual-local-2026-04-08.md)

## Troubleshooting
- `Invalid CSRF token.`
  - la cookie `XSRF-TOKEN` no fue obtenida primero o no se uso el valor rotado despues del login
- `No se encuentra el RefreshToken.`
  - el login previo no dejo cookie `refreshToken` en el mismo cookie jar
- `La entidad asociada al usuario no existe`
  - la base local no esta alineada o faltan migraciones/seeds
- errores por provider equivocado
  - levantar la API usando los scripts de [guia-ejecucion-por-provider.md](E:/DST/dev/AUT2Services/docs/guia-ejecucion-por-provider.md), no desde un launch profile mezclado

## Notas operativas
- El documento OpenAPI integrado sirve como contrato del backend.
- `Swagger UI` queda solo como apoyo temporal para exploracion manual y smoke tests.
- La validacion funcional repetible debe hacerse en Postman o una herramienta equivalente.
- El backend rota la cookie CSRF despues del login y despues del refresh token. La coleccion ya sincroniza esa rotacion automaticamente.
