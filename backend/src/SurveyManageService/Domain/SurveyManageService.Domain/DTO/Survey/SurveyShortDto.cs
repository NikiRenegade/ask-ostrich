namespace SurveyManageService.Domain.DTO;

public class SurveyShortDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public Guid AuthorGuid { get; set; }
    public DateTime CreatedAt { get; set; }
    public int QuestionCount { get; set; }
}