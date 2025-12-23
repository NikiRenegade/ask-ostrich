using SurveyManageService.Domain.DTO;

namespace SurveyManageService.Domain.Interfaces.Services;

public interface IShortUrlService
{
    Task<ShortUrlDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<ShortUrlDto?> GetByCodeAsync(string code, CancellationToken cancellationToken);

    Task<ShortUrlCreatedDto> AddAsync(CreateShortUrlDto request, CancellationToken cancellationToken);

    Task<string> GenerateShortCode(CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
