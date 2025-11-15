using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SurveyManageService.Domain.Interfaces.Consumers;

namespace SurveyManageService.Infrastructure.Messaging;

public class UserEventsBackgroundService: BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public UserEventsBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<IUserEventConsumer>();
        await consumer.StartAsync();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}