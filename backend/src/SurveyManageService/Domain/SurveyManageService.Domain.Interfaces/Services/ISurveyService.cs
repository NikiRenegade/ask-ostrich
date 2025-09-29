using SurveyManageService.Domain.DTO;

namespace SurveyManageService.Domain.Interfaces.Services;

public interface ISurveyService
{
    Task<IList<SurveyDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<SurveyDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<SurveyCreatedDto> AddAsync(CreateSurveyDto request, CancellationToken cancellationToken);

    Task<bool> UpdateAsync(UpdateSurveyDto request, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
