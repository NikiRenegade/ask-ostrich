namespace TelegramBotService.Domain.Dto;

public class QuestionDto
{
    public Guid Id { get; set; }
    public QuestionType Type { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public string InnerText { get; set; }
    public List<OptionDto> Options { get; set; }
}


public enum QuestionType
{
    Text = 0,
    SingleChoice,
    MultipleChoice
}