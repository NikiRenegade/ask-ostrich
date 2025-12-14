namespace SurveyManageService.Domain.DTO;

public class UpdateSurveyDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public Guid AuthorGuid { get; set; }
    public List<QuestionDto> Questions { get; set; } = [];
    public Guid? ShortUrlId { get; set; }
}