using System.Text;
using System.Text.Json;
using System.Collections.Concurrent;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SurveyManageService.Domain.Interfaces.Consumers;

namespace SurveyManageService.Infrastructure.Messaging;
public class RabbitMqEventConsumer : IEventConsumer
{
    private readonly Task<IChannel> _channelTask;
    private readonly ConcurrentDictionary<string, bool> _queues = new();

    public RabbitMqEventConsumer(Task<IChannel> channelTask)
    {
        _channelTask = channelTask;
    }

    public async Task SubscribeAsync<T>(string routingKey, string exchangeName, Func<T, Task> handleEvent)
    {
        var channel = await _channelTask;

        if (_queues.TryAdd(routingKey, true))
        {
            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            string queueName = $"manage_{routingKey.Replace(".", "_")}.queue";

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false);

            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var eventObj = JsonSerializer.Deserialize<T>(message);

                    if (eventObj != null)
                        await handleEvent(eventObj);

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Во время обработки сообщения произошла ошибка: {ex}");
                }
            };

            await channel.BasicConsumeAsync(queue: queueName, autoAck: false, consumer: consumer);
        }
    }
}