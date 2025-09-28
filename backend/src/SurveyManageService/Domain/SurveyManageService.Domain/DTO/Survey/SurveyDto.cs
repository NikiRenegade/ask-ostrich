namespace SurveyManageService.Domain.DTO;

public class SurveyDto
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public UserDto? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUpdateAt { get; set; }
    public List<QuestionDto> Questions { get; set; } = [];
}