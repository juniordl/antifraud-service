using System.Text.Json;
using Common.Messaging.Core.Interfaces;
using Confluent.Kafka;

namespace Common.Messaging.Kafka.Implementations;

public class KafkaEventBus(KafkaConfiguration configuration) : IEventBus
{
    public async Task PublishAsync<T>(T message, string topic)
    {
        var config = new ProducerConfig { BootstrapServers = configuration.Server };
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = JsonSerializer.Serialize(message) });
    }
}