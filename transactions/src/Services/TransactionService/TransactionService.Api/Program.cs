using System.Text.Json.Serialization;
using Common.Messaging.Core;
using Common.Messaging.Core.Interfaces;
using Common.Messaging.Kafka;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using TransactionServices.Application;
using TransactionServices.Application.Transaction.Events;
using TransactionServices.Extensions;
using TransactionServices.Infrastructure;
using TransactionServices.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddApplication();

var kafkaConfiguration = builder.Configuration.GetSection(KafkaConfiguration.SectionName).Get<KafkaConfiguration>();

builder.Services.AddInfrastructure(builder.Configuration, kafkaConfiguration ?? throw new InvalidOperationException());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TransactionDbContext>();
    dbContext.Database.Migrate();
}

app.MapEndpoints();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

var kafkaConsumer = app.Services.GetRequiredService<IEventConsumer>();
var cts = new CancellationTokenSource();
Task.Run(() => kafkaConsumer.ConsumeAsync<TransactionValidatedEvent>(cts.Token));

app.Run();