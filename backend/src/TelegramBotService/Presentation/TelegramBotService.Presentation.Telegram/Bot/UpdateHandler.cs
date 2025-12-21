using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBotService.Application.Handlers;

namespace TelegramBotService.Presentation.Telegram.Bot;

public class UpdateHandler : IUpdateHandler
{
    private readonly HandleUserInputUseCase _useCase;

    public UpdateHandler(HandleUserInputUseCase useCase)
    {
        _useCase = useCase;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
    {
        var (userId, text, action) = TelegramMapper.Map(update);

        var response = await _useCase.HandleAsync(userId, text, action);

        await bot.SendMessage(
            userId,
            response.Text,
            replyMarkup: TelegramMapper.Map(response.Actions),
            cancellationToken: ct);
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        Console.WriteLine(exception);
        return Task.CompletedTask;
    }
}