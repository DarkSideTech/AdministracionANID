# -------------------------------------------------
# Dark Side Tech
# Solution Name : AUT2Services
# Domain : Administracion version 1.17
# Date Generated File : 2025-12-03 11:34:56.415
# -------------------------------------------------
param(
    [string]$Environment = "Development"
)

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path $PSScriptRoot).Path

function Update-ScriptForProvider {
    param(
        [string]$Provider,
        [string]$ProjectPath,
        [string]$OutputPath
    )

    Write-Host "Generate $Provider script migration" -ForegroundColor Green
    $env:ASPNETCORE_ENVIRONMENT = $Environment
    $env:DB_PROVIDER = $Provider
    Remove-Item Env:ConnectionStrings__AUT2ServicesConnection -ErrorAction SilentlyContinue

    dotnet ef migrations script `
        --idempotent `
        --context AUT2ServicesContext `
        --project $ProjectPath `
        --startup-project $projectRoot `
        -o $OutputPath
}

Push-Location $projectRoot
try {
    Update-ScriptForProvider -Provider "postgresql" `
        -ProjectPath "..\AUT2Services.Infra.Migrations.PostgreSql\AUT2Services.Infra.Migrations.PostgreSql.csproj" `
        -OutputPath "..\AUT2Services.Infra.Migrations.PostgreSql\Migration_AUT2Services_PostgreSql.sql"

    Update-ScriptForProvider -Provider "sqlserver" `
        -ProjectPath "..\AUT2Services.Infra.Migrations.SqlServer\AUT2Services.Infra.Migrations.SqlServer.csproj" `
        -OutputPath "..\AUT2Services.Infra.Migrations.SqlServer\Migration_AUT2Services_SqlServer.sql"
}
finally {
    Pop-Location
}

