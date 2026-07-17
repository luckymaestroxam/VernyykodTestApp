FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY src/ ./
RUN dotnet publish "MigrationService/MigrationService.ConsoleApp/MigrationService.ConsoleApp.csproj" -c Release -o /app/publish --no-self-contained

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MigrationService.ConsoleApp.dll"]
