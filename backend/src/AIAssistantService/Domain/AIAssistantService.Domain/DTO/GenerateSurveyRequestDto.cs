namespace AIAssistantService.Domain.DTO;

public class GenerateSurveyRequestDto
{
    public required string Prompt { get; set; }

    public required string CurrentSurveyJson { get; set; } 
   
    public PromptType Type { get; set; } = PromptType.UpdateSurvey;
    
    public string? SurveyId { get; set; }
}

public enum PromptType
{    
    UpdateSurvey,
    Ask
}