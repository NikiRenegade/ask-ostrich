namespace TelegramBotService.Domain.Dto;

public class SurveyAnswerDto
{
    public Guid QuestionId { get; set; }
    public string QuestionTitle { get; set; } = "";
    public List<string> Values { get; set; } = [];
}