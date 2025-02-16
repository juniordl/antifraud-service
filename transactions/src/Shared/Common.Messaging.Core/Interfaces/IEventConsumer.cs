namespace Common.Messaging.Core.Interfaces;

public interface IEventConsumer
{
    Task ConsumeAsync<T>(CancellationToken cancellationToken);
}