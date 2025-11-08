FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AIAssistantService/Presentation/AIAssistantService.Presentation.API/AIAssistantService.Presentation.API.csproj", "src/AIAssistantService/Presentation/AIAssistantService.Presentation.API/"]
COPY ["src/AIAssistantService/Application/AIAssistantService.Application/AIAssistantService.Application.csproj", "src/AIAssistantService/Application/AIAssistantService.Application/"]
COPY ["src/AIAssistantService/Domain/AIAssistantService.Domain.Interfaces/AIAssistantService.Domain.Interfaces.csproj", "src/AIAssistantService/Domain/AIAssistantService.Domain.Interfaces/"]
COPY ["src/AIAssistantService/Domain/AIAssistantService.Domain/AIAssistantService.Domain.csproj", "src/AIAssistantService/Domain/AIAssistantService.Domain/"]
COPY ["src/AIAssistantService/Infrastructure/AIAssistantService.Infrastructure/AIAssistantService.Infrastructure.csproj", "src/AIAssistantService/Infrastructure/AIAssistantService.Infrastructure/"]
RUN dotnet restore "src/AIAssistantService/Presentation/AIAssistantService.Presentation.API/AIAssistantService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/AIAssistantService/Presentation/AIAssistantService.Presentation.API"
RUN dotnet build "AIAssistantService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AIAssistantService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AIAssistantService.Presentation.API.dll"]
