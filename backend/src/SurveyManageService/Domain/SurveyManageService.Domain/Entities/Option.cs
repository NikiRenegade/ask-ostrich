namespace SurveyManageService.Domain.Entities;

public class Option
{
    public required string Title { get; set; }
    public required string Value { get; set; }
    public int Order { get; set; } = 0;
    public bool IsCorrect { get; set; }
}
