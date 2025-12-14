namespace SurveyManageService.Domain.DTO;

public class ShortUrlCreatedDto
{
    public required Guid Id { get; set; }
    public required string Code { get; set; }
}
