using SurveyManageService.Domain.Entities;

namespace SurveyManageService.Domain.Interfaces.Repositories;

public interface ISurveyRepository: IRepository<Survey>
{
    Task<IList<Survey>> GetExistingByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
