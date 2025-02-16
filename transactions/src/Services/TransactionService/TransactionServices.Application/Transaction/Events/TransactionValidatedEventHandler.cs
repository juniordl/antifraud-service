using System.Data;
using Common.Messaging.Core.Interfaces;
using TransactionService.Domain.Enums;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;

namespace TransactionServices.Application.Transaction.Events;

public class TransactionValidatedEventHandler(ITransactionRepository transactionRepository): IEventHandler<TransactionValidatedEvent>
{
    public async Task HandleAsync(TransactionValidatedEvent message, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.Get(message.TransactionId);
        if (transaction is null)
            throw new EvaluateException("Transaction not found");

        transaction.Status = message.Approved ? TransactionStatus.Approved : TransactionStatus.Rejected;
        transaction.ModifiedAt = DateTime.UtcNow;
        
        await transactionRepository.Update(transaction);
    }
}