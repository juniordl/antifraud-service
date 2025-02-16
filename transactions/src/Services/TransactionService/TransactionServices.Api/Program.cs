using Common.Messaging.Core.Interfaces;
using Common.Messaging.Kafka;
using TransactionServices.Application;
using TransactionServices.Application.Transaction.Events;
using TransactionServices.Extensions;
using TransactionServices.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

var kafkaConfiguration = builder.Configuration.GetSection(KafkaConfiguration.SectionName).Get<KafkaConfiguration>();

builder.Services.AddInfrastructure(builder.Configuration, kafkaConfiguration ?? throw new InvalidOperationException());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

var kafkaConsumer = app.Services.GetRequiredService<IEventConsumer>();
var cts = new CancellationTokenSource();
Task.Run(() => kafkaConsumer.ConsumeAsync<TransactionValidatedEvent>(cts.Token));

app.Run();