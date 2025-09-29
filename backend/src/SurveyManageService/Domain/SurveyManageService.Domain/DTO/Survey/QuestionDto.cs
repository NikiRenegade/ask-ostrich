using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Domain.DTO;

public class QuestionDto
{    
    public QuestionType Type { get; set; }
    public required string Title { get; set; }
    public int Order { get; set; } = 0;
    public required string InnerText { get; set; }
    public List<Option> Options { get; set; } = [];
}