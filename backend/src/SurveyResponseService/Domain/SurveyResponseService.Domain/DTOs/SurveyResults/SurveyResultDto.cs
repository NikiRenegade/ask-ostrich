namespace SurveyResponseService.Domain.DTOs.SurveyResults
{
    public class SurveyResultDto
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime DatePassed { get; set; }
        public List<AnswerDto> Answers { get; set; } = new();
    }
}
