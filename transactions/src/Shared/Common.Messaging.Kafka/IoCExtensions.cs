
using Common.Messaging.Core.Interfaces;
using Common.Messaging.Kafka.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Messaging.Kafka;

public static class IoCExtensions
{
    public static IServiceCollection AddKafka(this IServiceCollection services, KafkaConfiguration configuration)
    {
        services.AddSingleton(configuration);
        
        services.AddScoped<IEventDispatcher, EventDispatcher>();
        
        services.AddSingleton<IEventBus, KafkaEventBus>();

        services.AddSingleton<IEventConsumer, KafkaEventConsumer>();

        return services;
    }
}