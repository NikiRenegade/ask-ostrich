FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SurveyManageService/Presentation/SurveyManageService.Presentation.API/SurveyManageService.Presentation.API.csproj", "src/SurveyManageService/Presentation/SurveyManageService.Presentation.API/"]
COPY ["src/SurveyManageService/Application/SurveyManageService.Application/SurveyManageService.Application.csproj", "src/SurveyManageService/Application/SurveyManageService.Application/"]
COPY ["src/SurveyManageService/Domain/SurveyManageService.Domain.Interfaces/SurveyManageService.Domain.Interfaces.csproj", "src/SurveyManageService/Domain/SurveyManageService.Domain.Interfaces/"]
COPY ["src/SurveyManageService/Domain/SurveyManageService.Domain/SurveyManageService.Domain.csproj", "src/SurveyManageService/Domain/SurveyManageService.Domain/"]
COPY ["src/SurveyManageService/Infrastructure/SurveyManageService.Infrastructure.Repositories/SurveyManageService.Infrastructure.Repositories.csproj", "src/SurveyManageService/Infrastructure/SurveyManageService.Infrastructure.Repositories/"]
COPY ["src/SurveyManageService/Infrastructure/SurveyManageService.Infrastructure.EntityFramework/SurveyManageService.Infrastructure.EntityFramework.csproj", "src/SurveyManageService/Infrastructure/SurveyManageService.Infrastructure.EntityFramework/"]
RUN dotnet restore "src/SurveyManageService/Presentation/SurveyManageService.Presentation.API/SurveyManageService.Presentation.API.csproj"
COPY . .
WORKDIR "/src/src/SurveyManageService/Presentation/SurveyManageService.Presentation.API"
RUN dotnet build "SurveyManageService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SurveyManageService.Presentation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SurveyManageService.Presentation.API.dll"]
