namespace TelegramBotService.Domain.Dto;

public class MySurveyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public bool IsPublished { get; set; }
    public DateTime CreatedAt { get; set; }
    public int QuestionCount { get; set; }
}