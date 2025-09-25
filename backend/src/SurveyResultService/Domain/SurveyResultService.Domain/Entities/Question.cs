namespace SurveyManageService.Domain.Entities;

public class Question
{
    public Guid Id { get; set; }
    public QuestionType Type { get; set; }
    public string Title { get; set; }
    public int Order { get; set; }
    public string InnerText { get; set; }
    public List<Option> Options { get; set; }
}
