namespace TransactionServices.Application.Transaction.Events;

public class TransactionEvaluatedEvent
{
    public Guid TransactionId { get; set; }
    public bool Approved { get; set; }
}