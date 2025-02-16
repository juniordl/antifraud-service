using System.Text.Json;
using AntiFraudService.Application.Events;
using AntiFraudService.Application.Interfaces;
using AntiFraudService.Application.Interfaces.Messaging;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace AntiFraudService.Infrastructure.Messaging;

public class KafkaEventConsumer: IEventConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaEventConsumer> _logger;
    private readonly ITransactionHandler _transactionHandler;

    public KafkaEventConsumer(ILogger<KafkaEventConsumer> logger, ITransactionHandler transactionHandler)
    {
        _logger = logger;
        _transactionHandler = transactionHandler;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "created-transaction-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe("created-transactions-topic");
    }
    
    public async Task ConsumeAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Kafka Consumer starting...");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    var transaction = JsonSerializer.Deserialize<TransactionCreatedEvent>(consumeResult.Message.Value);

                    if (transaction != null) await _transactionHandler.Handle(transaction);
                    _consumer.Commit(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("Kafka Consumer stopped.");
        }
        finally
        {
            _consumer.Close();
        }
    }
}