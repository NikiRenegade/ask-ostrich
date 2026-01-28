namespace TelegramBotService.Domain.Dto;

public record OptionDto(string Title, string Value, int Order, bool IsCorrect);