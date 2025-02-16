namespace Common.Messaging.Core.Interfaces;

public interface IEventDispatcher
{
    Task DispatchAsync<T>(T message, CancellationToken cancellationToken);
}