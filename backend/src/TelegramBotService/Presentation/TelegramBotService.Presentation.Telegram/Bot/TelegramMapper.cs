using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBotService.Application;

namespace TelegramBotService.Presentation.Telegram.Bot;

public static class TelegramMapper
{
    public static (long userId, string? text, string? action) Map(Update update)
    {
        if (update.Message != null)
        {
            return (
                update.Message.Chat.Id,
                update.Message.Text,
                null
            );
        }

        if (update.CallbackQuery != null)
        {
            return (
                update.CallbackQuery.Message!.Chat.Id,
                null,
                update.CallbackQuery.Data
            );
        }

        return default;
    }
    
    public static ReplyMarkup? Map(IReadOnlyList<AppAction>? actions)
    {
        if (actions == null || actions.Count == 0)
            return null;

        var buttons = actions
            .Select(a =>
                a.Url != null
                    ? InlineKeyboardButton.WithUrl(a.Label, a.Url)
                    : InlineKeyboardButton.WithCallbackData(a.Label, a.Id!))
            .ToArray();
        
        var rows = new List<InlineKeyboardButton[]>();
        for (int i = 0; i < buttons.Length; i += 2)
        {
            if (i + 1 < buttons.Length)
                rows.Add(new[] { buttons[i], buttons[i + 1] });
            else
                rows.Add(new[] { buttons[i] });
        }

        return new InlineKeyboardMarkup(rows);
    }
}