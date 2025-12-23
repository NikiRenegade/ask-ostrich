using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Domain.Interfaces.Repositories;

public interface ISurveyRepository: IRepository<Survey>
{
    Task AddWithShortUrlAsync(Survey survey, ShortUrl shortUrl, CancellationToken cancellationToken);
    Task<Survey?> GetByShortUrlCodeAsync(string shortCode, CancellationToken cancellationToken);
    Task<IList<Survey>> GetExistingByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
