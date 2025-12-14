using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Domain.Interfaces.Repositories;

public interface IShortUrlRepository: IRepository<ShortUrl>
{
    Task<ShortUrl?> GetByCodeAsync(string code, CancellationToken cancellationToken);
}
