FROM mcr.microsoft.com/dotnet/runtime:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TelegramBotService/Presentation/TelegramBotService.Presentation.Telegram/TelegramBotService.Presentation.Telegram.csproj", "src/TelegramBotService/Presentation/TelegramBotService.Presentation.Telegram/"]
COPY ["src/TelegramBotService/Application/TelegramBotService.Application/TelegramBotService.Application.csproj", "src/TelegramBotService/Application/TelegramBotService.Application/"]
COPY ["src/TelegramBotService/Domain/TelegramBotService.Domain.Interfaces/TelegramBotService.Domain.Interfaces.csproj", "src/TelegramBotService/Domain/TelegramBotService.Domain.Interfaces/"]
COPY ["src/TelegramBotService/Domain/TelegramBotService.Domain/TelegramBotService.Domain.csproj", "src/TelegramBotService/Domain/TelegramBotService.Domain/"]
COPY ["src/SurveyResponseService/Domain/SurveyResponseService.Domain.Interfaces/SurveyResponseService.Domain.Interfaces.csproj", "src/SurveyResponseService/Domain/SurveyResponseService.Domain.Interfaces/"]
COPY ["src/SurveyResponseService/Domain/SurveyResponseService.Domain/SurveyResponseService.Domain.csproj", "src/SurveyResponseService/Domain/SurveyResponseService.Domain/"]
COPY ["src/TelegramBotService/Infrastructure/TelegramBotService.Infrastructure/TelegramBotService.Infrastructure.csproj", "src/TelegramBotService/Infrastructure/TelegramBotService.Infrastructure/"]
RUN dotnet restore "src/TelegramBotService/Presentation/TelegramBotService.Presentation.Telegram/TelegramBotService.Presentation.Telegram.csproj"
COPY . .
WORKDIR "/src/src/TelegramBotService/Presentation/TelegramBotService.Presentation.Telegram"
RUN dotnet build "TelegramBotService.Presentation.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TelegramBotService.Presentation.Telegram.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TelegramBotService.Presentation.Telegram.dll"]