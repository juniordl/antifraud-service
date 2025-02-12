namespace TransactionServices.Application.Transaction.Dto;

public class TransactionDto
{
    public Guid SourceAccountId { get; set; }
    public Guid TransferAccountId { get; set; }
    public int TransferType { get; set; }
    public double Value { get; set; }
}