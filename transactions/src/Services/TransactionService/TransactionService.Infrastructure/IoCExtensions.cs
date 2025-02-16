using Common.Messaging.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionServices.Application.Interfaces.Infrastructure.Repositories;
using TransactionServices.Infrastructure.Database;
using TransactionServices.Infrastructure.Repositories;

namespace TransactionServices.Infrastructure;

public static class IoCExtensions
{
    private const string ConnectionName = "PostgresDb";
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, KafkaConfiguration kafkaConfiguration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionName);
        services.AddDbContext<TransactionDbContext>(op => op.UseNpgsql(connectionString));
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddKafka(kafkaConfiguration); 
        return services;
    }
}