FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
#COPY ["manage.api/manage.api.csproj", "manage.api/"]
#COPY ["manage.aplication/manage.aplication.csproj", "manage.aplication/"]
#COPY ["manage.business/manage.core.csproj", "manage.business/"]
#COPY ["manage.data/manage.infra.csproj", "manage.data/"]
COPY . .
WORKDIR "/src/manage.api"
RUN dotnet restore "./manage.api.csproj"
RUN dotnet build "./manage.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./manage.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "manage.api.dll"]
