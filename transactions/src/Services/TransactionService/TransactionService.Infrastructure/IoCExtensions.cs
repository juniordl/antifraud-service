using Common.Messaging.Kafka;
using Confluent.Kafka;
using HealthChecks.Kafka;
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
        
        services.AddHealthChecks()
            .AddKafka(new KafkaHealthCheckOptions
            {
                Configuration = new ProducerConfig() { BootstrapServers = kafkaConfiguration.Server},
                Topic = kafkaConfiguration.ProducerTopic
            });
        
        services.AddHealthChecks().AddNpgSql(connectionString ?? throw new InvalidCastException());
        
        return services;
    }
}