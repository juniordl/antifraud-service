using MediatR;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Application.Transaction.Dto;

namespace TransactionServices.Application.Transaction.Commands;

public class CreateTransactionCommandHandler(ITransactionRepository transactionRepository)
    : IRequestHandler<CreateTransactionCommandRequest, TransactionDto>
{
    public async Task<TransactionDto> Handle(CreateTransactionCommandRequest request, CancellationToken cancellationToken)
    {
        var entity = new TransactionService.Domain.Transaction()
        {
            SourceAccountId = request.Transaction.SourceAccountId,
            TransferAccountId = request.Transaction.TransferAccountId,
            TransferType = request.Transaction.TransferType,
            Value = request.Transaction.Value
        };
        
        await transactionRepository.Create(entity);
        return request.Transaction;
    }
}

public class CreateTransactionCommandRequest(TransactionDto transaction) : IRequest<TransactionDto>
{
    public TransactionDto Transaction { get; } = transaction;
}