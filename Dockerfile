FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Directory.Packages.props ./
COPY src/ ./
RUN dotnet restore AUT2Services.Services.API/AUT2Services.Services.API.csproj
RUN dotnet publish AUT2Services.Services.API/AUT2Services.Services.API.csproj --configuration Release --output /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Sandbox
ENV ASPNETCORE_URLS=http://+:10000

COPY --from=build /app/publish ./

EXPOSE 10000
ENTRYPOINT ["dotnet", "AUT2Services.Services.API.dll"]
