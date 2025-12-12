using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Domain.Interfaces.Repositories
{
    public interface ISurveyResultRepository : IRepository<SurveyResult>
    {
        Task<IList<SurveyResult>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    }
}
