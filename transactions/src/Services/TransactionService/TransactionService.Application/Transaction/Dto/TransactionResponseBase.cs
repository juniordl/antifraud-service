using TransactionService.Domain.Enums;

namespace TransactionServices.Application.Transaction.Dto;

public abstract class TransactionResponseBase
{
    public Guid TransactionId { get; set; }
    public Guid SourceAccountId { get; set; }
    public Guid TransferAccountId { get; set; }
    public int TransferType { get; set; }
    public double Value { get; set; }
    public TransactionStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
}