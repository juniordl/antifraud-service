using AntiFraudService.Application.Interfaces.Cache;
using AntiFraudService.Infrastructure.Cache;
using Common.Messaging.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace AntiFraudService.Infrastructure;

public static class IoCExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, KafkaConfiguration kafkaConfiguration, RedisConfiguration redisConfiguration)
    {
        services.AddSingleton(redisConfiguration);
        services.AddSingleton<ICacheRepository, RedisCacheRepository>();
        services.AddKafka(kafkaConfiguration);
        return services;
    }
}