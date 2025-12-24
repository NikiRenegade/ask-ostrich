namespace TelegramBotService.Domain.Dto;

public record AuthStatusDto(bool Completed, Guid UserId, string UserName, string FirstName, string LastName);