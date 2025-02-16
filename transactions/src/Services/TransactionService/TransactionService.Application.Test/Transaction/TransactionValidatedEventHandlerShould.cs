using System.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionService.Domain.Enums;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Application.Transaction.Events;

namespace TransactionService.Application.Test.Transaction;

public class TransactionValidatedEventHandlerShould
{
    private readonly Mock<ITransactionRepository> _transactionRepository;
    private readonly TransactionValidatedEventHandler _handler;

    public TransactionValidatedEventHandlerShould()
    {
        _transactionRepository = new Mock<ITransactionRepository>();
        Mock<ILogger<TransactionValidatedEventHandler>> logger = new();
        _handler = new TransactionValidatedEventHandler(_transactionRepository.Object, logger.Object);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateTransactionStatusToApproved()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var transaction = new TransactionService.Domain.Transaction
        {
            Id = transactionId,
            Status = TransactionStatus.Pending,
            ModifiedAt = DateTime.UtcNow
        };
        _transactionRepository.Setup(repo => repo.Get(transactionId)).ReturnsAsync(transaction);

        var message = new TransactionValidatedEvent
        {
            TransactionId = transactionId,
            Approved = true
        };

        // Act
        await _handler.HandleAsync(message, CancellationToken.None);

        // Assert
        transaction.Status.Should().Be(TransactionStatus.Approved);
        _transactionRepository.Verify(repo => repo.Update(transaction), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldUpdateTransactionStatusToRejected()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        var transaction = new TransactionService.Domain.Transaction
        {
            Id = transactionId,
            Status = TransactionStatus.Pending,
            ModifiedAt = DateTime.UtcNow
        };
        _transactionRepository.Setup(repo => repo.Get(transactionId)).ReturnsAsync(transaction);

        var message = new TransactionValidatedEvent
        {
            TransactionId = transactionId,
            Approved = false
        };

        // Act
        await _handler.HandleAsync(message, CancellationToken.None);

        // Assert
        transaction.Status.Should().Be(TransactionStatus.Rejected);
        _transactionRepository.Verify(repo => repo.Update(transaction), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowExceptionWhenTransactionNotFound()
    {
        // Arrange
        var transactionId = Guid.NewGuid();
        _transactionRepository.Setup(repo => repo.Get(transactionId))!.ReturnsAsync((TransactionService.Domain.Transaction)null);

        var message = new TransactionValidatedEvent
        {
            TransactionId = transactionId,
            Approved = true
        };

        // Act
        var act = async () => await _handler.HandleAsync(message, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<EvaluateException>().WithMessage("Transaction not found");
        _transactionRepository.Verify(repo => repo.Update(It.IsAny<TransactionService.Domain.Transaction>()), Times.Never);
    }
}