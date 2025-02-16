using AntiFraudService.Application.Services;
using Common.Messaging.Core.Interfaces;
using Microsoft.Extensions.Logging;
using TransactionServices.Application.Transaction.Events;

namespace AntiFraudService.Application.Events;

public class TransactionCreatedEventHandler(IEventBus eventBus, IAntiFraudControlService antiFraudControlService, ILogger<TransactionCreatedEventHandler> logger) : IEventHandler<TransactionCreatedEvent>
{
    public async Task HandleAsync(TransactionCreatedEvent message, CancellationToken cancellationToken)
    {
        var approved = await antiFraudControlService.IsApprovedTransaction(message);

        var result = new TransactionValidatedEvent()
        {
            TransactionId = message.TransactionId,
            Approved = approved
        };

        await eventBus.PublishAsync(result, "evaluated-transactions-topic");
        
        logger.LogInformation("TransactionCreatedEvent handled transaction {Id}", message.TransactionId);
    }
}