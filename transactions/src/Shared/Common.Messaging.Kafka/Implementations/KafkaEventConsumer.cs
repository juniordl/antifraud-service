using System.Text.Json;
using Common.Messaging.Core.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Common.Messaging.Kafka.Implementations;

public class KafkaEventConsumer : IEventConsumer
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<KafkaEventConsumer> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public KafkaEventConsumer(ILogger<KafkaEventConsumer> logger, KafkaConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        var config = new ConsumerConfig
        {
            BootstrapServers = configuration.Server,
            GroupId = configuration.ConsumerGroup,
            AutoOffsetReset = Enum.Parse<AutoOffsetReset>(configuration.Offset),
            EnableAutoCommit = false,
            MetadataMaxAgeMs = 10000
        };
            
        _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        _consumer.Subscribe(configuration.ConsumerTopic);
    }

    public async Task ConsumeAsync<T>(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Kafka Consumer starting...");

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = _consumer.Consume(cancellationToken);
                    var @event = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);
                    
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var eventDispatcher = scope.ServiceProvider.GetRequiredService<IEventDispatcher>();
                        await eventDispatcher.DispatchAsync(@event, cancellationToken);
                    }
                    
                    _consumer.Commit(consumeResult);
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, "Error on consuming");
                }
            }
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogError(ex, "Kafka Consumer stopped.");
        }
        finally
        {
            _consumer.Close();
        }
    }
}