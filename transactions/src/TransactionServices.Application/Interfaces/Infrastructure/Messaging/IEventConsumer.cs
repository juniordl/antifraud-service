namespace TransactionServices.Application.Interfaces.Infrastructure.Messaging;

public interface IEventConsumer
{
    Task ConsumeAsync(CancellationToken cancellationToken);
}