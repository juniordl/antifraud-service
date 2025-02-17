using Common.Messaging.Core;
using Common.Messaging.Core.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Application.Transaction.Commands;
using TransactionServices.Application.Transaction.Dto;
using TransactionServices.Application.Transaction.Events;

namespace TransactionService.Application.Test.Transaction;

public class CreateTransactionCommandHandlerShould
{
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly Mock<IEventBus> _eventBus;
    private readonly CreateTransactionCommandHandler _handler;

    public CreateTransactionCommandHandlerShould()
    {
        _transactionRepository = new Mock<ITransactionRepository>();
        _eventBus = new Mock<IEventBus>();
        Mock<ILogger<CreateTransactionCommandHandler>> logger = new();
        var kafkaConfiguration = new KafkaConfiguration()
            { Server = "kafka:29092", ProducerTopic = "created-transactions-topic" };
        _handler = new CreateTransactionCommandHandler(_transactionRepository.Object, _eventBus.Object, logger.Object, kafkaConfiguration);
    }

    [Fact]
    public async Task Handle_ShouldCreateTransaction()
    {
        // Arrange
        var transactionDto = new TransactionDto
        {
            SourceAccountId = Guid.NewGuid(),
            TransferAccountId = Guid.NewGuid(),
            TransferType = 1,
            Value = 100.00
        };
        var request = new CreateTransactionCommandRequest(transactionDto);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _transactionRepository.Verify(repo => repo.Create(It.IsAny<TransactionService.Domain.Transaction>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldPublishTransactionCreatedEvent()
    {
        // Arrange
        var transactionDto = new TransactionDto
        {
            SourceAccountId = Guid.NewGuid(),
            TransferAccountId = Guid.NewGuid(),
            TransferType = 1,
            Value = 100.00
        };
        var request = new CreateTransactionCommandRequest(transactionDto);

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _eventBus.Verify(bus => bus.PublishAsync(It.IsAny<TransactionCreatedEvent>(), "created-transactions-topic"), Times.Once);
    }
}