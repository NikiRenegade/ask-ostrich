namespace TelegramBotService.Application;

public class AppResponse
{
    public string Text { get; init; } = "";
    public IReadOnlyList<AppAction>? Actions { get; set; }
}
    