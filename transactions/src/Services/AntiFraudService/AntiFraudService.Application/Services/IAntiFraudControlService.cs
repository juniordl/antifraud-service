using TransactionServices.Application.Transaction.Events;

namespace AntiFraudService.Application.Services;

public interface IAntiFraudControlService
{
    Task<bool> IsApprovedTransaction(TransactionCreatedEvent transaction);
}