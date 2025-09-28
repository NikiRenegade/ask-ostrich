namespace SurveyManageService.Domain.DTO;

public class CreateSurveyDto
{
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid AuthorGuid { get; set; }
    public List<QuestionDto> Questions { get; set; } = [];
}