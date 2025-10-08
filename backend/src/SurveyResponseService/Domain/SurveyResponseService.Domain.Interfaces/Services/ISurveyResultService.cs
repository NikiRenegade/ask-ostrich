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
    }
}
