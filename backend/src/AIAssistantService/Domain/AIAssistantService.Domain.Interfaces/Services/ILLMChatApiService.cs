namespace AIAssistantService.Domain.Interfaces.Services
{
    public interface ILLMChatApiService
    {
        Task<string> GetResponse(string prompt, CancellationToken cancellationToken = default);
        IAsyncEnumerable<string> GetResponseStream(string prompt, CancellationToken cancellationToken = default);
    }
}
