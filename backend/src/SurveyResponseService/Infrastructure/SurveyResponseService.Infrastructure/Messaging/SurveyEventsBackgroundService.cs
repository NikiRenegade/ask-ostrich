using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SurveyResponseService.Domain.Interfaces.Consumers;

namespace SurveyResponseService.Infrastructure.Messaging;

public class SurveyEventsBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public SurveyEventsBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<ISurveyEventConsumer>();
        await consumer.StartAsync();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}