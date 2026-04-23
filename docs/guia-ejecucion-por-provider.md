# Guia corta de operacion

## Regla base
La clave oficial para seleccionar el motor SQL es `DB_PROVIDER`.

Valores soportados:
- `postgresql`
- `sqlserver`

La API resuelve la conexion en este orden:
1. `ConnectionStrings__AUT2ServicesConnection`
2. `ConnectionStrings:AUT2ServicesConnectionPostgreSql` o `ConnectionStrings:AUT2ServicesConnectionSqlServer`

`ASPNETCORE_DB_PROVIDER` ya no es necesario.

## 1. Cambiar PostgreSQL / SQL Server

### PostgreSQL
```powershell
$env:DB_PROVIDER = "postgresql"
.\scripts\update-db-postgresql.ps1
.\scripts\run-postgresql.ps1
```

### SQL Server
```powershell
$env:DB_PROVIDER = "sqlserver"
.\scripts\update-db-sqlserver.ps1
.\scripts\run-sqlserver.ps1
```

### Override puntual de conexion
Si necesitas otra instancia sin tocar `appsettings.Development.json`:

```powershell
$env:DB_PROVIDER = "sqlserver"
$env:ConnectionStrings__AUT2ServicesConnection = "Server=localhost,1433;Database=AUT2Services;User ID=sa;Password=Password123;TrustServerCertificate=true;Encrypt=false"
dotnet run --no-launch-profile --project .\src\AUT2Services.Services.API\AUT2Services.Services.API.csproj
```

## 2. Activar o desactivar proyeccion Mongo

La proyeccion Mongo es independiente del motor SQL. El journal durable sigue quedando en SQL y Mongo solo recibe una proyeccion.

Configuracion en [appsettings.Development.json](E:/DST/dev/AUT2Services/src/AUT2Services.Services.API/appsettings.Development.json):

```json
"MongoAuditProjection": {
  "Enabled": true,
  "ConnectionString": "mongodb://localhost:27017",
  "DatabaseName": "AUT2ServicesTraceability",
  "CollectionName": "audit_trail",
  "BatchSize": 100,
  "IntervalSeconds": 5
}
```

### Activar
```powershell
$env:MongoAuditProjection__Enabled = "true"
dotnet run --no-launch-profile --project .\src\AUT2Services.Services.API\AUT2Services.Services.API.csproj
```

### Desactivar
```powershell
$env:MongoAuditProjection__Enabled = "false"
dotnet run --no-launch-profile --project .\src\AUT2Services.Services.API\AUT2Services.Services.API.csproj
```

### Docker local validado
```powershell
docker run --name mongo-db -d -p 27017:27017 -v mongo_data:/data/db mongo
```

## 3. Regenerar migraciones y scripts

Los helpers viven en [AUT2Services.Services.API](E:/DST/dev/AUT2Services/src/AUT2Services.Services.API):
- [AddMigration.ps1](E:/DST/dev/AUT2Services/src/AUT2Services.Services.API/AddMigration.ps1)
- [UpdateScriptDatabaseMigration.ps1](E:/DST/dev/AUT2Services/src/AUT2Services.Services.API/UpdateScriptDatabaseMigration.ps1)

### Crear una migracion en ambos providers
```powershell
Set-Location .\src\AUT2Services.Services.API
.\AddMigration.ps1 -MigrationName NombreDeLaMigracion
```

### Regenerar scripts SQL de ambos providers
```powershell
Set-Location .\src\AUT2Services.Services.API
.\UpdateScriptDatabaseMigration.ps1
```

## Comprobacion minima
1. Levantar PostgreSQL o SQL Server.
2. Levantar Mongo si la proyeccion esta activa.
3. Ejecutar migraciones.
4. Levantar la API.
5. Validar `http://localhost:5002/openapi/v1.json`.
6. Ejecutar la coleccion descrita en [openapi-postman.md](E:/DST/dev/AUT2Services/docs/openapi-postman.md).

## Observaciones
- Si `launchSettings.json` interfiere, usar `--no-launch-profile`.
- Si cambias de motor SQL, no necesitas tocar codigo.
- Si desactivas Mongo, la trazabilidad sigue quedando en SQL `AuditOutbox`.
- El estado operativo base esta resumido en [estado-actual-local-2026-04-08.md](E:/DST/dev/AUT2Services/docs/estado-actual-local-2026-04-08.md).
