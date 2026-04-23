param(
    [string]$Environment = "Development",
    [string]$ConnectionString = "Server=localhost;Port=5432;Database=AUT2Services;User Id=postgres;Password=postgres"
)

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path

Push-Location $projectRoot
try {
    $env:ASPNETCORE_ENVIRONMENT = $Environment
    $env:ASPNETCORE_DB_PROVIDER = "postgresql"
    $env:DB_PROVIDER = "postgresql"
    $env:ConnectionStrings__AUT2ServicesConnection = $ConnectionString

    dotnet ef database update `
        --project ".\src\AUT2Services.Infra.Migrations.PostgreSql\AUT2Services.Infra.Migrations.PostgreSql.csproj" `
        --startup-project ".\src\AUT2Services.Services.API\AUT2Services.Services.API.csproj"
}
finally {
    Pop-Location
}
