namespace SurveyManageService.Domain.DTO;

public class CreateShortUrlDto
{
    public required Guid SurveyId { get; set; }
    public required string OriginUrl { get; set; }
}
