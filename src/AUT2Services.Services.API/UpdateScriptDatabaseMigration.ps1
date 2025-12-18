# -------------------------------------------------
# Dark Side Tech
# Solution Name : AUT2Services
# Domain : Administracion version 1.17
# Date Generated File : 2025-12-03 11:34:56.415
# -------------------------------------------------
Write-Host "Generate PostgreSql Script migration" -ForegroundColor Green
$env:ASPNETCORE_DB_PROVIDER = "postgresql"
dotnet ef migrations script --idempotent --startup-project ../AUT2Services.Services.API/ --context AUT2ServicesContext --idempotent -o ../AUT2Services.Infra.Migrations.PostgreSql/Migration_AUT2Services_PostgreSql.sql

Write-Host "Generate Sql Server Script migration" -ForegroundColor Green
$env:ASPNETCORE_DB_PROVIDER = "sqlserver"
dotnet ef migrations script --idempotent --startup-project ../AUT2Services.Services.API/ --context AUT2ServicesContext --idempotent -o ../AUT2Services.Infra.Migrations.SqlServer/Migration_AUT2Services_SqlServer.sql

