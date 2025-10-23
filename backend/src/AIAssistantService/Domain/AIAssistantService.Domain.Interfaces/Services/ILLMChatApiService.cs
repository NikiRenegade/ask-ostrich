namespace AIAssistantService.Domain.Interfaces.Services
{
    public interface ILLMChatApiService
    {
        Task<string> GetResponse(string prompt, CancellationToken cancellationToken = default);
    }
}
