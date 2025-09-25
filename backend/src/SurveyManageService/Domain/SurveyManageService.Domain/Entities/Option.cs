namespace SurveyManageService.Domain.Entities;

public class Option
{
    public string Title { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }
    public bool IsCorrect { get; set; }
}
