using Common.Messaging.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Messaging.Kafka.Implementations;

public class EventDispatcher(IServiceProvider serviceProvider) : IEventDispatcher
{
    public async Task DispatchAsync<T>(T message, CancellationToken cancellationToken)
    {
        var handler = serviceProvider.GetService<IEventHandler<T>>();

        if (handler == null)
        {
            throw new InvalidOperationException($"Handler not found for event {typeof(T).Name}");
        }

        await handler.HandleAsync(message, cancellationToken);
    }
}