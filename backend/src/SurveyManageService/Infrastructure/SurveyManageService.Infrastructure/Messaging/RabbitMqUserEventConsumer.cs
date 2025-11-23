using SurveyManageService.Domain.DTO;
using SurveyManageService.Domain.Interfaces.Consumers;
using SurveyManageService.Domain.Interfaces.Services;

namespace SurveyManageService.Infrastructure.Messaging;

public class RabbitMqUserEventConsumer : IUserEventConsumer
{
    private readonly IEventConsumer _consumer;
    private readonly IUserService _userService;
    private const string ExchangeName = "user-events";

    public RabbitMqUserEventConsumer(IEventConsumer consumer, IUserService userService)
    {
        _consumer = consumer;
        _userService = userService;
    }

    public async Task StartAsync()
    {
        await _consumer.SubscribeAsync<CreateUserDto>("user.created", ExchangeName, async @event =>
        {
            await _userService.AddAsync(@event, CancellationToken.None);
        });

        await _consumer.SubscribeAsync<UpdateUserDto>("user.updated", ExchangeName, async @event =>
        {
            await _userService.UpdateAsync(@event, CancellationToken.None);
        });

        await _consumer.SubscribeAsync<Guid>("user.deleted", ExchangeName, async @event =>
        {
            await _userService.DeleteAsync(@event, CancellationToken.None);
        });
    }
}