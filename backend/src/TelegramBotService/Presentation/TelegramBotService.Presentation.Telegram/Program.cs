using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using TelegramBotService.Application.Commands;
using TelegramBotService.Application.Commands.Auth;
using TelegramBotService.Application.Commands.Surveys;
using TelegramBotService.Application.Handlers;
using TelegramBotService.Application.Interfaces;
using TelegramBotService.Infrastructure.Services;
using TelegramBotService.Presentation.Telegram.Bot;


Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration;

        services.AddSingleton<IUserSessionStore, InMemoryUserSessionStore>();
        services.AddSingleton<HandleUserInputUseCase>();
        
        var frontendBaseUrl = configuration["Frontend:BaseUrl"];
        services.AddSingleton<IAuthFrontendUrlProvider>(new AuthFrontendUrlProvider(frontendBaseUrl));
        
        var apiBase = configuration["Api:BaseUrl"];
        services.AddHttpClient<IAuthApi, AuthApi>(c => c.BaseAddress = new Uri(apiBase));
        services.AddHttpClient<ISurveyApi, SurveyApi>(c => c.BaseAddress = new Uri(apiBase));

        // Telegram Bot

        services.AddSingleton<ITelegramBotClient>(_ =>
        {
            var token = Environment.GetEnvironmentVariable("BOT_TOKEN");
            if (string.IsNullOrWhiteSpace(token))
                throw new InvalidOperationException("BOT_TOKEN environment variable is not set");

            return new TelegramBotClient(token);
        });

        services.AddSingleton<IUpdateHandler, UpdateHandler>();
        services.AddHostedService<TelegramBotHostedService>();
        services.AddSingleton<IUserCommand, StartCommand>();
        services.AddSingleton<IUserCommand, LoginCommand>();
        services.AddSingleton<IUserCommand, StartSurveyCommand>();
        services.AddSingleton<IUserCommand, AuthPendingCommand>();
        services.AddSingleton<IUserCommand, ProfileCommand>();
        services.AddSingleton<IUserCommand, EnterSurveyGuidCommand>();
        services.AddSingleton<IUserCommand, SurveyAnswerCommand>();
        services.AddSingleton<IUserCommand, CompletedSurveysCommand>();
        services.AddSingleton<IUserCommand, ViewPassedSurveyCommand>();
        services.AddSingleton<IUserCommand, MySurveysCommand>();
    })
    .Build()
    .Run();
