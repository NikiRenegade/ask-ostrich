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
            var chatresponse = client.GetStreamingResponseAsync(prompt, cancellationToken: cancellationToken);
            var result = await ProcessResponseAsync(chatresponse);

            return result;
        }

        public async Task<string> ProcessResponseAsync(IAsyncEnumerable<ChatResponseUpdate> response)
        {
            string chatresponse = string.Empty;
            await foreach (var item in response)
            {
                Console.Write(item);
                chatresponse = chatresponse + item.ToString();
            }
            return chatresponse;
        }
    }
}
