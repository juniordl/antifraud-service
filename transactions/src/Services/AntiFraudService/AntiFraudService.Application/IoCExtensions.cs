using AntiFraudService.Application.Events;
using AntiFraudService.Application.Interfaces;
using AntiFraudService.Application.Services;
using Common.Messaging.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TransactionServices.Application.Transaction.Events;

namespace AntiFraudService.Application;

public static class IoCExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAntiFraudControlService, AntiFraudControlService>();
        services.AddScoped<IEventHandler<TransactionCreatedEvent>, TransactionCreatedEventHandler>();
        return services;
    }
}