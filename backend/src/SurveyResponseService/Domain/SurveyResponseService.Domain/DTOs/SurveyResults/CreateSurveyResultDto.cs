namespace SurveyResponseService.Domain.DTOs.SurveyResults
{
    public class CreateSurveyResultDto
    {
        public Guid UserId { get; set; }
        public Guid SurveyId { get; set; }
        public DateTime DatePassed { get; set; }
        public IList<AnswerDto>? Answers { get; set; }
    }
}
