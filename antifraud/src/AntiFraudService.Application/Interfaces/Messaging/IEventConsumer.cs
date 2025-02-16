namespace AntiFraudService.Application.Interfaces.Messaging;

public interface IEventConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}