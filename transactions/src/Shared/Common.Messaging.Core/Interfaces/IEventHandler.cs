namespace Common.Messaging.Core.Interfaces;

public interface IEventHandler<in T>
{
    Task HandleAsync(T message, CancellationToken cancellationToken);
}