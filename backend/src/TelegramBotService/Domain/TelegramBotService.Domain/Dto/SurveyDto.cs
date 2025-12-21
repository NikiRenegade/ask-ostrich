namespace TelegramBotService.Domain.Dto;

public class SurveyDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<QuestionDto> Questions { get; set; }
}