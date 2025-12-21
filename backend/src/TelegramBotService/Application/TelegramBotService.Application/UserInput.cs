namespace TelegramBotService.Application;

public class UserInput
{
    public string? Text { get; init; }
    public string? Action { get; init; }
    public long ChatId { get; init; }

    public bool IsCommand(string command) => Text == command;
    public bool IsAction(string action) => Action == action;
}