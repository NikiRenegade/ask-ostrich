namespace SurveyResponseService.Domain.DTOs.SurveyResults
{
    public class SurveyResultCreatedDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? GuestId { get; set; }
        public string? DisplayName { get; set; } = null!;
        public Guid SurveyId { get; set; }
        public DateTime DatePassed { get; set; }
        public IList<AnswerDto>? Answers { get; set; }
    }
}
