using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionServices.Application.Interfaces.Infrastructure.Messaging;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Infrastructure.Database;
using TransactionServices.Infrastructure.Messaging;
using TransactionServices.Infrastructure.Repositories;

namespace TransactionServices.Infrastructure;

public static class IoCExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresDb");
        services.AddDbContext<TransactionDbContext>(op => op.UseNpgsql(connectionString));
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddSingleton<IEventBus, KafkaEventBus>();
        services.AddSingleton<IEventConsumer, KafkaEventConsumer>();
        return services;
    }
}