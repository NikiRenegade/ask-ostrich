using SurveyResponseService.Domain.Entities;

namespace SurveyResponseService.Domain.Interfaces.Repositories
{
    public interface ISurveyResultRepository : IRepository<SurveyResult>
    {
        Task<IList<SurveyResult>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
        Task<IList<SurveyResult>> GetBySurveyIdAsync(Guid surveyId, CancellationToken cancellationToken);
    }
}
