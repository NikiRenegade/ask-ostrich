using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Metadata;
using SecurityService.Domain.Interfaces.Publishers;
using SecurityService.Domain.Events;
using RabbitMQ.Client;
using System.Threading.Tasks;
namespace SecurityService.Infrastructure.Messaging;

public class RabbitMqUserEventPublisher : IUserEventPublisher
{
    private readonly IEventPublisher _eventPublisher;
    private const string ExchangeName = "user-events";
    

    public RabbitMqUserEventPublisher(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    
    public Task PublishUserCreated(UserCreatedEvent userCreatedEvent)
    {
        return _eventPublisher.PublishAsync(
            userCreatedEvent,
            routingKey: "user.created",
            exchangeName: ExchangeName
        );
    }

    public Task PublishUserUpdated(UserUpdatedEvent userUpdatedEvent)
    {
        return _eventPublisher.PublishAsync(
            @userUpdatedEvent,
            routingKey: "user.updated",
            exchangeName: ExchangeName
        );
    }

    public Task PublishUserDeleted(Guid userId)
    {
        return _eventPublisher.PublishAsync(
            userId,
            routingKey: "user.deleted",
            exchangeName: ExchangeName
        );
    }
}