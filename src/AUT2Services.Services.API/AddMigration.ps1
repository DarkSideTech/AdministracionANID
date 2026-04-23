# -------------------------------------------------
# Dark Side Tech
# Solution Name : AUT2Services
# Domain : Administracion version 1.17
# Date Generated File : 2025-12-03 11:34:56.415
# -------------------------------------------------
param(
 [parameter(mandatory)]
 [string] $MigrationName,
 [string] $Environment = "Development"
)

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path $PSScriptRoot).Path

function Add-MigrationForProvider {
    param(
        [string] $Provider,
        [string] $ProjectPath
    )

    Write-Host "Adding $Provider migration" -ForegroundColor Green
    $env:ASPNETCORE_ENVIRONMENT = $Environment
    $env:DB_PROVIDER = $Provider
    Remove-Item Env:ConnectionStrings__AUT2ServicesConnection -ErrorAction SilentlyContinue

    dotnet ef migrations add $MigrationName `
        --context AUT2ServicesContext `
        --project $ProjectPath `
        --startup-project $projectRoot
}

Push-Location $projectRoot
try {
    Add-MigrationForProvider -Provider "postgresql" -ProjectPath "..\AUT2Services.Infra.Migrations.PostgreSql\AUT2Services.Infra.Migrations.PostgreSql.csproj"
    Add-MigrationForProvider -Provider "sqlserver" -ProjectPath "..\AUT2Services.Infra.Migrations.SqlServer\AUT2Services.Infra.Migrations.SqlServer.csproj"
}
finally {
    Pop-Location
}

