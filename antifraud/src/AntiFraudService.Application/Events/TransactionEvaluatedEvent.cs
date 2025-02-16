namespace AntiFraudService.Application.Events;

public class TransactionEvaluatedEvent
{
    public Guid TransactionId { get; set; }
    public bool Approved { get; set; }
}