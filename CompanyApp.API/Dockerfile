# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CompanyApp.API/CompanyApp.API.csproj", "CompanyApp.API/"]
COPY ["CompanyApp.Infrastructure/CompanyApp.Infrastructure.csproj", "CompanyApp.Infrastructure/"]
COPY ["CompanyApp.Database/CompanyApp.Database.csproj", "CompanyApp.Database/"]
COPY ["CompanyApp.Domain/CompanyApp.Domain.csproj", "CompanyApp.Domain/"]
RUN dotnet restore "./CompanyApp.API/CompanyApp.API.csproj"
COPY . .
WORKDIR "/src/CompanyApp.API"
RUN dotnet build "./CompanyApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CompanyApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CompanyApp.API.dll"]