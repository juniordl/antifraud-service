using AntiFraudService.Application.Events;

namespace AntiFraudService.Application.Interfaces;

public interface ITransactionHandler
{
    Task Handle(TransactionCreatedEvent @event);
}