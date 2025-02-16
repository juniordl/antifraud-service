namespace TransactionServices.Application.Transaction.Events;

public class TransactionValidatedEvent
{
    public Guid TransactionId { get; set; }
    public bool Approved { get; set; }
}