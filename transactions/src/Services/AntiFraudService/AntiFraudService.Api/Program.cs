using AntiFraudService.Application;
using AntiFraudService.Infrastructure;
using AntiFraudService.Infrastructure.Cache;
using Common.Messaging.Core;
using Common.Messaging.Core.Interfaces;
using Common.Messaging.Kafka;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TransactionServices.Application.Transaction.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();

var kafkaConfiguration = builder.Configuration.GetSection(KafkaConfiguration.SectionName).Get<KafkaConfiguration>() 
                         ?? throw new InvalidOperationException() ;

var redisConfiguration = builder.Configuration.GetSection(RedisConfiguration.SectionName).Get<RedisConfiguration>() 
                         ?? throw new InvalidOperationException() ;

builder.Services.AddInfrastructure(kafkaConfiguration, redisConfiguration);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

var kafkaConsumer = app.Services.GetRequiredService<IEventConsumer>();
var cts = new CancellationTokenSource();
Task.Run(() => kafkaConsumer.ConsumeAsync<TransactionCreatedEvent>(cts.Token));

app.Run();