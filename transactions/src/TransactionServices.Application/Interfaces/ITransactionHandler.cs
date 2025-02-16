using TransactionServices.Application.Transaction.Events;

namespace TransactionServices.Application.Interfaces;

public interface ITransactionHandler
{
    Task Handle(TransactionEvaluatedEvent @event);
}