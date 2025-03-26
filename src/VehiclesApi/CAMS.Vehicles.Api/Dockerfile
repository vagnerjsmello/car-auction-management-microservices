# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8090
EXPOSE 8091


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/CAMS.VehiclesApi/CAMS.Vehicles.Api.csproj", "src/CAMS.VehiclesApi/"]
RUN dotnet restore "./src/CAMS.VehiclesApi/CAMS.Vehicles.Api.csproj"
COPY . .
WORKDIR "/src/src/CAMS.VehiclesApi"
RUN dotnet build "./CAMS.Vehicles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CAMS.Vehicles.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CAMS.Vehicles.Api.dll"]