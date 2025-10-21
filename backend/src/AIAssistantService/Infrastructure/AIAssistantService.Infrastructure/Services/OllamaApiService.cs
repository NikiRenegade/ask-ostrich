using AIAssistantService.Domain.Interfaces.Services;
using Microsoft.Extensions.AI;
using OllamaSharp;

namespace AIAssistantService.Infrastructure.Services
{
    public class OllamaApiService: ILLMChatApiService
    {
        public async Task<string> GetResponse(string prompt, CancellationToken cancellationToken = default)
        {
            using var client = new OllamaApiClient(new Uri("http://localhost:11434/"), "gpt-oss:20b");
            var result = await client.GetResponseAsync(prompt, cancellationToken: cancellationToken);
            return result.ToString();
        }
    }
}
