namespace TelegramBotService.Domain.Dto;

public record AuthStatusDto(bool Completed, Guid? UserId, string? UserName = null, string? FirstName = null, string? LastName = null);