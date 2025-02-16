namespace TransactionServices.Application.Transaction.Events;

public class TransactionCreatedEvent
{
    public Guid TransactionId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid TransferAccountId { get; set; }
    public double Value { get; set; }
}