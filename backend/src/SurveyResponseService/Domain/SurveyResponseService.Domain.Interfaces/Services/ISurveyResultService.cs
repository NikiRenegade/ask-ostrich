using SurveyResponseService.Domain.DTOs.Survey;
using SurveyResponseService.Domain.DTOs.SurveyResults;

namespace SurveyResponseService.Domain.Interfaces.Services
{
    public interface ISurveyResultService
    {
        Task<IList<SurveyResultDto>> GetAllAsync(CancellationToken cancellationToken);

        Task<SurveyResultDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<SurveyResultCreatedDto> AddAsync(CreateSurveyResultDto request, CancellationToken cancellationToken);

        Task<bool> UpdateAsync(UpdateSurveyResultDto request, CancellationToken cancellationToken);

        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<IList<PassedSurveyDto>> GetPassedSurveysByUserIdAsync(Guid userId, CancellationToken cancellationToken);

        Task<PassedSurveyDto?> GetLatestBySurveyIdAndUserIdAsync(Guid surveyId, Guid userId, CancellationToken cancellationToken);

        Task<IList<SurveyResultDto>> GetBySurveyIdAsync(Guid surveyId, CancellationToken cancellationToken);
    }
}
