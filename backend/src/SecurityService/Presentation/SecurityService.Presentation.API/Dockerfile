FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SecurityService/Presentation/SecurityService.Presentation.API/SecurityService.Presentation.API.csproj", "src/SecurityService/Presentation/SecurityService.Presentation.API/"]
COPY ["src/SecurityService/Application/SecurityService.Application/SecurityService.Application.csproj", "src/SecurityService/Application/SecurityService.Application/"]
COPY ["src/SecurityService/Domain/SecurityService.Domain.Interfaces/SecurityService.Domain.Interfaces.csproj", "src/SecurityService/Domain/SecurityService.Domain.Interfaces/"]
COPY ["src/SecurityService/Domain/SecurityService.Domain/SecurityService.Domain.csproj", "src/SecurityService/Domain/SecurityService.Domain/"]
COPY ["src/SecurityService/Infrastructure/SecurityService.Infrastructure.EntityFramework/SecurityService.Infrastructure.EntityFramework.csproj", "src/SecurityService/Infrastructure/SecurityService.Infrastructure.EntityFramework/"]
COPY ["src/SecurityService/Infrastructure/SecurityService.Infrastructure/SecurityService.Infrastructure.csproj", "src/SecurityService/Infrastructure/SecurityService.Infrastructure/"]
COPY ["src/SecurityService/Infrastructure/SecurityService.Infrastructure.Repositories/SecurityService.Infrastructure.Repositories.csproj", "src/SecurityService/Infrastructure/SecurityService.Infrastructure.Repositories/"]
RUN dotnet restore "src/SecurityService/Presentation/SecurityService.Presentation.API/SecurityService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/SecurityService/Presentation/SecurityService.Presentation.API"
RUN dotnet build "SecurityService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SecurityService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecurityService.Presentation.API.dll"]
