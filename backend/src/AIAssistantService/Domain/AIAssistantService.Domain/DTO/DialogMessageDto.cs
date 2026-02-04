namespace AIAssistantService.Domain.DTO;

public class DialogMessageDto
{
    public required string Content { get; set; }
    public required bool IsUserMessage { get; set; }
    public required DateTime Timestamp { get; set; }
}
