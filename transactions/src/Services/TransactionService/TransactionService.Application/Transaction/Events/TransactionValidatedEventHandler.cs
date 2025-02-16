using System.Data;
using Common.Messaging.Core.Interfaces;
using Microsoft.Extensions.Logging;
using TransactionService.Domain.Enums;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;

namespace TransactionServices.Application.Transaction.Events;

public class TransactionValidatedEventHandler(
    ITransactionRepository transactionRepository,
    ILogger<TransactionValidatedEventHandler> logger) : IEventHandler<TransactionValidatedEvent>
{
    public async Task HandleAsync(TransactionValidatedEvent message, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.Get(message.TransactionId);
        if (transaction is null)
            throw new EvaluateException("Transaction not found");

        transaction.Status = message.Approved ? TransactionStatus.Approved : TransactionStatus.Rejected;
        transaction.ModifiedAt = DateTime.UtcNow;

        await transactionRepository.Update(transaction);

        logger.LogInformation("TransactionValidatedEvent handled transaction {Id} and status updated to: {Status}",
            transaction.Id, transaction.Status.ToString());
    }
}