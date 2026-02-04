using AIAssistantService.Domain.DTO;

namespace AIAssistantService.Domain.Interfaces.Services;

public interface IDialogHistoryService
{
    Task SaveMessagesAsync(string surveyId, IEnumerable<DialogMessageDto> messages, CancellationToken cancellationToken = default);
    Task<List<DialogMessageDto>> GetDialogHistoryAsync(string surveyId, CancellationToken cancellationToken = default);
    Task ClearDialogHistoryAsync(string surveyId, CancellationToken cancellationToken = default);
}
