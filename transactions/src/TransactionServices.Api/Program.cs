using TransactionServices.Application;
using TransactionServices.Application.Interfaces.Infrastructure.Messaging;
using TransactionServices.Extensions;
using TransactionServices.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapEndpoints();

var kafkaConsumer = app.Services.GetRequiredService<IEventConsumer>();
var cts = new CancellationTokenSource();
Task.Run(() => kafkaConsumer.ConsumeAsync(cts.Token));

app.Run();