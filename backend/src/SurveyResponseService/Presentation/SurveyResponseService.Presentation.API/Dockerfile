FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SurveyResponseService/Presentation/SurveyResponseService.Presentation.API/SurveyResponseService.Presentation.API.csproj", "src/SurveyResponseService/Presentation/SurveyResponseService.Presentation.API/"]
COPY ["src/SurveyResponseService/Application/SurveyResponseService.Application/SurveyResponseService.Application.csproj", "src/SurveyResponseService/Application/SurveyResponseService.Application/"]
COPY ["src/SurveyResponseService/Domain/SurveyResponseService.Domain.Interfaces/SurveyResponseService.Domain.Interfaces.csproj", "src/SurveyResponseService/Domain/SurveyResponseService.Domain.Interfaces/"]
COPY ["src/SurveyResponseService/Domain/SurveyResponseService.Domain/SurveyResponseService.Domain.csproj", "src/SurveyResponseService/Domain/SurveyResponseService.Domain/"]
COPY ["src/SurveyResponseService/Infrastructure/SurveyResponseService.Infrastructure.Repositories/SurveyResponseService.Infrastructure.Repositories.csproj", "src/SurveyResponseService/Infrastructure/SurveyResponseService.Infrastructure.Repositories/"]
COPY ["src/SurveyResponseService/Infrastructure/SurveyResponseService.Infrastructure.EntityFramework/SurveyResponseService.Infrastructure.EntityFramework.csproj", "src/SurveyResponseService/Infrastructure/SurveyResponseService.Infrastructure.EntityFramework/"]
RUN dotnet restore "src/SurveyResponseService/Presentation/SurveyResponseService.Presentation.API/SurveyResponseService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/SurveyResponseService/Presentation/SurveyResponseService.Presentation.API"
RUN dotnet build "SurveyResponseService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SurveyResponseService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveyResponseService.Presentation.API.dll"]
