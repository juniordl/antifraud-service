using System.Text.Json;
using Confluent.Kafka;
using TransactionServices.Application.Interfaces.Infrastructure.Messaging;

namespace TransactionServices.Infrastructure.Messaging;

public class KafkaEventBus: IEventBus
{
    private readonly string _bootstrapServers = "localhost:9092";
    
    public async Task PublishAsync<T>(T message, string topic)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        //TODO: Add key
        using var producer = new ProducerBuilder<Null, string>(config).Build();
        var jsonMessage = JsonSerializer.Serialize(message);
        
        await producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
    }
}