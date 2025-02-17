using Common.Messaging.Core;
using Common.Messaging.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Application.Transaction.Dto;
using TransactionServices.Application.Transaction.Events;

namespace TransactionServices.Application.Transaction.Commands;

public class CreateTransactionCommandHandler(ITransactionRepository transactionRepository, IEventBus eventBus, ILogger<CreateTransactionCommandHandler> logger, KafkaConfiguration kafkaConfiguration)
    : IRequestHandler<CreateTransactionCommandRequest, CreateTransactionCommandResponse>
{
    public async Task<CreateTransactionCommandResponse> Handle(CreateTransactionCommandRequest request, CancellationToken cancellationToken)
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
        
        await eventBus.PublishAsync(@event, kafkaConfiguration.ProducerTopic);
        
        return new CreateTransactionCommandResponse(entity);
    }
}

public class CreateTransactionCommandRequest(TransactionDto transaction) : IRequest<CreateTransactionCommandResponse>
{
    public TransactionDto Transaction { get; } = transaction;
}

public class CreateTransactionCommandResponse() : TransactionResponseBase
{
    public CreateTransactionCommandResponse(TransactionService.Domain.Transaction entity) : this()
    {
        TransactionId = entity.Id;
        SourceAccountId = entity.SourceAccountId;
        TransferAccountId = entity.TransferAccountId;
        TransferType = entity.TransferType;
        Value = entity.Value;
        Status = entity.Status;
        CreatedAt = entity.CreatedAt;
        ModifiedAt = entity.ModifiedAt;
    }
}