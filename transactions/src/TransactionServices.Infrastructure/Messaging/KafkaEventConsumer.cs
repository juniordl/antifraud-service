using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TransactionServices.Application.Interfaces;
using TransactionServices.Application.Interfaces.Infrastructure.Messaging;
using TransactionServices.Application.Transaction.Events;

namespace TransactionServices.Infrastructure.Messaging;

public class KafkaEventConsumer: IEventConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaEventConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public KafkaEventConsumer(ILogger<KafkaEventConsumer> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "evaluated-transaction-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false
        };
        
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe("evaluated-transactions-topic");
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
                    var transaction = JsonSerializer.Deserialize<TransactionEvaluatedEvent>(consumeResult.Message.Value);

                    //TODO: Improve dependency inyecction
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var transactionHandler = scope.ServiceProvider.GetRequiredService<ITransactionHandler>();
                        if (transaction != null) await transactionHandler.Handle(transaction);
                    }
                    
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