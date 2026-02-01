using AIAssistantService.Domain.DTO;

namespace AIAssistantService.Domain.Interfaces.Services;

public interface IDialogHistoryService
{
    Task SaveMessageAsync(string surveyId, DialogMessageDto message, CancellationToken cancellationToken = default);
    Task<List<DialogMessageDto>> GetDialogHistoryAsync(string surveyId, CancellationToken cancellationToken = default);
    Task ClearDialogHistoryAsync(string surveyId, CancellationToken cancellationToken = default);
}
