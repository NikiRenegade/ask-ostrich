using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace TelegramBotService.Presentation.Telegram.Bot;

public class TelegramBotHostedService : BackgroundService
{
    private readonly ITelegramBotClient _bot;
    private readonly IUpdateHandler _handler;

    public TelegramBotHostedService(ITelegramBotClient bot, IUpdateHandler handler)
    {
        _bot = bot;
        _handler = handler;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bot.StartReceiving(
            _handler,
            cancellationToken: stoppingToken);

        return Task.CompletedTask;
    }
}