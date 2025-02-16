using AntiFraudService.Application;
using AntiFraudService.Infrastructure;
using AntiFraudService.Infrastructure.Cache;
using Common.Messaging.Core.Interfaces;
using Common.Messaging.Kafka;
using TransactionServices.Application.Transaction.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();

var kafkaConfiguration = builder.Configuration.GetSection(KafkaConfiguration.SectionName).Get<KafkaConfiguration>() 
                         ?? throw new InvalidOperationException() ;

var redisConfiguration = builder.Configuration.GetSection(RedisConfiguration.SectionName).Get<RedisConfiguration>() 
                         ?? throw new InvalidOperationException() ;

builder.Services.AddInfrastructure(kafkaConfiguration, redisConfiguration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

var kafkaConsumer = app.Services.GetRequiredService<IEventConsumer>();
var cts = new CancellationTokenSource();
Task.Run(() => kafkaConsumer.ConsumeAsync<TransactionCreatedEvent>(cts.Token));

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}