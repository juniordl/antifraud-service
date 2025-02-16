using AntiFraudService.Application.Interfaces.Cache;
using AntiFraudService.Application.Interfaces.Messaging;
using AntiFraudService.Infrastructure.Cache;
using AntiFraudService.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace AntiFraudService.Infrastructure;

public static class IoCExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IEventConsumer, KafkaEventConsumer>();
        services.AddSingleton<IEventBus, KafkaEventBus>();
        services.AddSingleton<ICacheRepository, RedisCacheRepository>();
        return services;
    }
}