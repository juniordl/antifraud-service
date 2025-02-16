using Common.Messaging.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Application.Transaction.Dto;
using TransactionServices.Application.Transaction.Events;

namespace TransactionServices.Application.Transaction.Commands;

public class CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IEventBus eventBus, ILogger<CreateTransactionCommandHandler> logger)
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
        
        logger.LogInformation("Transaction created with id: {Id}", entity.Id);

        var @event = new TransactionCreatedEvent()
        {
            TransactionId = entity.Id,
            SourceAccountId = entity.SourceAccountId,
            TransferAccountId = entity.TransferAccountId,
            Value = entity.Value
        };
        
        await eventBus.PublishAsync(@event, "created-transactions-topic");
        
        return request.Transaction;
    }
}

public class CreateTransactionCommandRequest(TransactionDto transaction) : IRequest<TransactionDto>
{
    public TransactionDto Transaction { get; } = transaction;
}