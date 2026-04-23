param(
    [string]$Environment = "Development",
    [string]$ConnectionString = "Server=localhost,1433;Database=AUT2Services;User ID=sa;Password=Password123;TrustServerCertificate=true;Encrypt=false"
)

$ErrorActionPreference = "Stop"
$projectRoot = (Resolve-Path (Join-Path $PSScriptRoot "..")).Path

Push-Location $projectRoot
try {
    $env:ASPNETCORE_ENVIRONMENT = $Environment
    $env:ASPNETCORE_DB_PROVIDER = "sqlserver"
    $env:DB_PROVIDER = "sqlserver"
    $env:ConnectionStrings__AUT2ServicesConnection = $ConnectionString

    dotnet run --project ".\src\AUT2Services.Services.API\AUT2Services.Services.API.csproj" --no-build --no-launch-profile
}
finally {
    Pop-Location
}
