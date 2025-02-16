namespace Common.Messaging.Core.Interfaces;

public interface IEventBus
{
    Task PublishAsync<T>(T message, string topic);
}