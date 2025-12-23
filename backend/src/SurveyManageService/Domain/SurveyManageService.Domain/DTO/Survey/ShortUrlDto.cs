namespace SurveyManageService.Domain.DTO;

public class ShortUrlDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
    public required string OriginUrl { get; set; }
}