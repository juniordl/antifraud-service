using MediatR;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Application.Transaction.Dto;

namespace TransactionServices.Application.Transaction.Queries;

public class GetTransactionQueryHandler(ITransactionRepository transactionRepository): IRequestHandler<GetTransactionQueryRequest, GetTransactionQueryResponse>
{
    public async Task<GetTransactionQueryResponse> Handle(GetTransactionQueryRequest request, CancellationToken cancellationToken)
    {
        var entity = await transactionRepository.Get(request.TransactionId);
        return new GetTransactionQueryResponse()
        {
            TransactionId = entity.Id,
            SourceAccountId = entity.SourceAccountId,
            TransferAccountId = entity.TransferAccountId,
            TransferType = entity.TransferType,
            Value = entity.Value,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            ModifiedAt = entity.ModifiedAt
        };
    }
}


public class GetTransactionQueryRequest(Guid transactionId) : IRequest<GetTransactionQueryResponse>
{
    public Guid TransactionId { get; } = transactionId;
}

public class GetTransactionQueryResponse(): TransactionResponseBase;