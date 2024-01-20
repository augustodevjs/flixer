FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Flixer.Catalog.Api/Flixer.Catalog.Api.csproj", "src/Flixer.Catalog.Api/"]
RUN dotnet restore "./src/Flixer.Catalog.Api/./Flixer.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/src/Flixer.Catalog.Api"
RUN dotnet build "./Flixer.Catalog.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Flixer.Catalog.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Flixer.Catalog.Api.dll"]