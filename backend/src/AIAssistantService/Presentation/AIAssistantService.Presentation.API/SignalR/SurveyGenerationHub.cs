using System.Threading.Channels;
using AIAssistantService.Domain.DTO;
using AIAssistantService.Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace AIAssistantService.Presentation.API.SignalR;

public class LLMHub: Hub
{
    private readonly ILLMClientService _llmClientService;

    public LLMHub(ILLMClientService llmClientService)
    {
        _llmClientService = llmClientService;
    }

    public async Task GenerateSurvey(GenerateSurveyRequestDto request)
    {
        try
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Prompt))
            {
                throw new InvalidOperationException("Prompt is required");
            }
            
            var result = _llmClientService.GenerateSurveyAsync(request.Prompt, request.CurrentSurveyJson);

            int maxProgress = 60; // секунд работает модель
            int step = 10; // каждые 10 процентов отправляем прогресс
            int delay = maxProgress / step * 1000; // задержка между отправками прогресса

            for (int i = step; i <= maxProgress; i += step)
            {
                if (result.IsCompleted) break;
                
                await Clients.Caller.SendAsync("Progress", Math.Min(i, 95));
                await Task.Delay(delay);
            }

            
            await Clients.Caller.SendAsync("Progress", 100);
            await Clients.Caller.SendAsync("Completed", result.Result);
        }
        catch (Exception e)
        {
            await Clients.Caller.SendAsync("Error", e.Message);
        }
        
    }
    
    public async Task AskLLM(GenerateSurveyRequestDto request)
    {
        try
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Prompt))
            {
                throw new InvalidOperationException("Prompt is required");
            }
            
            var result = _llmClientService.AskLLMAsync(request.Prompt, request.CurrentSurveyJson);

            int maxProgress = 30; // секунд работает модель
            int step = 10; // каждые 10 процентов отправляем прогресс
            int delay = maxProgress / step * 1000; // задержка между отправками прогресса

            for (int i = step; i <= maxProgress; i += step)
            {
                if (result.IsCompleted) break;
                
                await Clients.Caller.SendAsync("Progress", Math.Min(i, 95));
                await Task.Delay(delay);
            }

            
            await Clients.Caller.SendAsync("Progress", 100);
            await Clients.Caller.SendAsync("Completed", result.Result);
        }
        catch (Exception e)
        {
            await Clients.Caller.SendAsync("Error", e.Message);
        }
        
    }
    
    public async Task AskLLMStream(GenerateSurveyRequestDto request)
    {
        var cts = new CancellationTokenSource();

        try
        {
            
            var progressTask = Task.Run(async () =>
            {
                try
                {
                    int maxProgress = 30; // секунд работает модель
                    int step = 10; // каждые 10 процентов отправляем прогресс
                    int delay = maxProgress / step * 1000; // задержка между отправками прогресса

                    for (int i = step; i <= maxProgress; i += step)
                    {
                        if (cts.Token.IsCancellationRequested) break;

                        await Clients.Caller.SendAsync("Progress", Math.Min(i, 95));
                        await Task.Delay(delay);
                    }
                }
                catch (TaskCanceledException){}
            });
            
            await foreach (var chunk in _llmClientService.AskLLMStreamAsync(
                               request.Prompt,
                               request.CurrentSurveyJson))
            {
                await Clients.Caller.SendAsync("Next", chunk);
                await Task.Delay(100); // псевдо задержка для посимвольного вывода

            }

            cts.Cancel();
            await Clients.Caller.SendAsync("Progress", 100);
            await Clients.Caller.SendAsync("Completed");
        }
        catch (Exception e)
        {
            cts.Cancel();
            await Clients.Caller.SendAsync("Error", e.Message);
        }    
    }
}