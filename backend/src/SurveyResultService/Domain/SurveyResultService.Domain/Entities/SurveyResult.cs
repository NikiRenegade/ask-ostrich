namespace SurveyManageService.Domain.Entities;

public class SurveyResult
{
    public Guid UserId { get; set; }
    public Guid SurveyId { get; set; }
    public DateTime DatePassed { get; set; }
}
