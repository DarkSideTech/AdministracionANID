# -------------------------------------------------
# Dark Side Tech
# Solution Name : AUT2Services
# Domain : Administracion version 1.17
# Date Generated File : 2025-12-03 11:34:56.415
# -------------------------------------------------
param(
 [parameter(mandatory)] 
 [string] $MigrationName)

Write-Host "Adding PostgreSql migration" -ForegroundColor Green

$env:ASPNETCORE_DB_PROVIDER = "postgresql"
dotnet ef migrations add $migrationName --context AUT2ServicesContext --project ../AUT2Services.Infra.Migrations.PostgreSql

Write-Host "Adding Sql Server migration" -ForegroundColor Green

$env:ASPNETCORE_DB_PROVIDER = "sqlserver"
dotnet ef migrations add $migrationName --context AUT2ServicesContext --project ../AUT2Services.Infra.Migrations.SqlServer

