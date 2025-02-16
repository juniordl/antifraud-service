using System.Data;
using TransactionService.Domain.Enums;
using TransactionServices.Application.Interfaces;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;

namespace TransactionServices.Application.Transaction.Events;

public class TransactionEvaluatedEventHandler(ITransactionRepository transactionRepository): ITransactionHandler
{
    public async Task Handle(TransactionEvaluatedEvent @event)
    {
        var transaction = await transactionRepository.Get(@event.TransactionId);
        if (transaction is null)
            throw new EvaluateException("Transaction not found");

        transaction.Status = @event.Approved ? TransactionStatus.Approved : TransactionStatus.Rejected;
        transaction.ModifiedAt = DateTime.UtcNow;
        
        await transactionRepository.Update(transaction);
    }
}