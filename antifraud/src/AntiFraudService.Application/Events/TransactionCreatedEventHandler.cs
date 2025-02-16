using AntiFraudService.Application.Interfaces;
using AntiFraudService.Application.Interfaces.Cache;
using AntiFraudService.Application.Interfaces.Messaging;
using AntiFraudService.Application.Services;

namespace AntiFraudService.Application.Events;

public class TransactionCreatedEventHandler(IEventBus eventBus, ICacheRepository cacheRepository) : ITransactionHandler
{
    private readonly AntiFraudControlService _antiFraudControlService = new(cacheRepository);
    
    public async Task Handle(TransactionCreatedEvent @event)
    {
        
        var approved = await _antiFraudControlService.IsApprovedTransaction(@event);

        var result = new TransactionEvaluatedEvent()
        {
            TransactionId = @event.TransactionId,
            Approved = approved
        };

        await eventBus.PublishAsync(result, "evaluated-transactions-topic");
    }
}