namespace SurveyResponseService.Domain.Interfaces.Consumers;

public interface IEventConsumer
{
    Task SubscribeAsync<T>(string routingKey, string exchangeName, Func<T, Task> handleEvent);
}