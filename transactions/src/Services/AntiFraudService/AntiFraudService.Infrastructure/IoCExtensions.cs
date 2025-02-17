using AntiFraudService.Application.Interfaces.Cache;
using AntiFraudService.Infrastructure.Cache;
using Common.Messaging.Core;
using Common.Messaging.Kafka;
using Confluent.Kafka;
using HealthChecks.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AntiFraudService.Infrastructure;

public static class IoCExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, KafkaConfiguration kafkaConfiguration, RedisConfiguration redisConfiguration)
    {
        services.AddSingleton(redisConfiguration);
        services.AddSingleton<ICacheRepository, RedisCacheRepository>();
        services.AddKafka(kafkaConfiguration);

        services.AddHealthChecks()
            .AddKafka(new KafkaHealthCheckOptions
            {
                Configuration = new ProducerConfig() { BootstrapServers = kafkaConfiguration.Server},
                Topic = kafkaConfiguration.ProducerTopic
            });
        
        services.AddHealthChecks()
            .AddCheck(RedisConfiguration.SectionName, new RedisHealthCheck(redisConfiguration), failureStatus: HealthStatus.Unhealthy);

        
        return services;
    }
}