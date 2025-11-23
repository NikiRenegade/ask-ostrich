using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Domain.Interfaces.Repositories;

public interface ISurveyRepository: IRepository<Survey>
{
    Task<IList<Survey>> GetExsistingByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
