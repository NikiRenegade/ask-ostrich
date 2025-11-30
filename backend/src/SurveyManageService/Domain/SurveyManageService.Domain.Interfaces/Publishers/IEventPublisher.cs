namespace SurveyManageService.Domain.Interfaces.Publishers;

public interface IEventPublisher
{
    Task PublishAsync<T>(
        T @event, string routingKey, string exchangeName);
}
