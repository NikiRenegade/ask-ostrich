namespace SurveyManageService.Domain.DTO;

public class CreateSurveyDto
{
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public Guid AuthorGuid { get; set; }
    public required string OriginUrl { get; set; }
    public List<CreateQuestionDto> Questions { get; set; } = [];
}