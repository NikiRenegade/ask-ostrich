using AIAssistantService.Domain.Interfaces.Services;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OllamaSharp;

namespace AIAssistantService.Infrastructure.Services
{
    public class OllamaApiService: ILLMChatApiService
    {
        private readonly IConfiguration _configuration;

        public OllamaApiService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GetResponse(string prompt, CancellationToken cancellationToken = default)
        {
            var baseUrl = _configuration["Ollama:BaseUrl"];
            var model = _configuration["Ollama:Model"];
            
            using var client = new OllamaApiClient(new Uri(baseUrl!), model!);
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
