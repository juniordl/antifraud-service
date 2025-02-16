namespace AntiFraudService.Application.Interfaces.Messaging;

public interface IEventBus
{
    Task PublishAsync<T>(T message, string topic);
}