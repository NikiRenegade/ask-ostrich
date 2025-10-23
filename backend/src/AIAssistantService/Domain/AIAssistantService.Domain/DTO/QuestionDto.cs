namespace AIAssistantService.Domain.DTO;

public class QuestionDto
{    
    public QuestionType Type { get; set; }
    public required string Title { get; set; }
    public int Order { get; set; } = 0;
    public required string InnerText { get; set; }
    public List<OptionDto> Options { get; set; } = [];
}

public enum QuestionType
{
    Text = 0,
    SingleChoice,
    MultipleChoice
}