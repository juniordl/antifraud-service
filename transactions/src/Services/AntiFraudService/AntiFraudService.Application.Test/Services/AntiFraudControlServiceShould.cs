using AntiFraudService.Application.Interfaces.Cache;
using AntiFraudService.Application.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TransactionServices.Application.Transaction.Events;

namespace AntiFraudService.Application.Test.Services;

public class AntiFraudControlServiceShould
    {
        private readonly Mock<ICacheRepository> _cacheRepository;
        private readonly AntiFraudControlService _service;

        public AntiFraudControlServiceShould()
        {
            _cacheRepository = new Mock<ICacheRepository>();
            Mock<ILogger<AntiFraudControlService>> logger = new();
            _service = new AntiFraudControlService(_cacheRepository.Object, logger.Object);
        }

        [Fact]
        public async Task IsApprovedTransaction_ShouldReturnFalse_WhenMaxValueExceeded()
        {
            // Arrange
            var transaction = new TransactionCreatedEvent
            {
                Value = 3000 // Exceeds MaxTransactionValue
            };

            // Act
            var result = await _service.IsApprovedTransaction(transaction);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsApprovedTransaction_ShouldReturnFalse_WhenAccumulatedValueExceeded()
        {
            // Arrange
            var transaction = new TransactionCreatedEvent
            {
                SourceAccountId = Guid.NewGuid(),
                Value = 5000
            };
            _cacheRepository.Setup(repo => repo.GetAsync(transaction.SourceAccountId.ToString()))
                .ReturnsAsync(18000); 

            // Act
            var result = await _service.IsApprovedTransaction(transaction);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task IsApprovedTransaction_ShouldReturnTrue_WhenValuesAreWithinLimits()
        {
            // Arrange
            var transaction = new TransactionCreatedEvent
            {
                SourceAccountId = Guid.NewGuid(),
                Value = 1000
            };
            _cacheRepository.Setup(repo => repo.GetAsync(transaction.SourceAccountId.ToString()))
                .ReturnsAsync(5000);
            
            _cacheRepository.Setup(repo => repo.GetTimeToLiveAsync(transaction.SourceAccountId.ToString()))
                .ReturnsAsync(TimeSpan.FromMinutes(60));

            // Act
            var result = await _service.IsApprovedTransaction(transaction);

            // Assert
            result.Should().BeTrue();
        }
    }