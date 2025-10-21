namespace AIAssistantService.Domain.DTO;

public class GeneratedSurveyDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;   
    public List<QuestionDto> Questions { get; set; } = [];
}