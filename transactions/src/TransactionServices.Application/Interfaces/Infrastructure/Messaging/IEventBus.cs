namespace TransactionServices.Application.Interfaces.Infrastructure.Messaging;

public interface IEventBus
{
    Task PublishAsync<T>(T message, string topic);
}