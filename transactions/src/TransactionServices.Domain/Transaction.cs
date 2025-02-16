using TransactionService.Domain.Enums;

namespace TransactionService.Domain;

public class Transaction
{
    public Transaction()
    {
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
        Status = TransactionStatus.Pending;
    }
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid TransferAccountId { get; set; }
    public int TransferType { get; set; }
    public double Value { get; set; }
    public TransactionStatus Status { get; set; }
}