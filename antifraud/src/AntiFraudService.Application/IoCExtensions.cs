using AntiFraudService.Application.Events;
using AntiFraudService.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AntiFraudService.Application;

public static class IoCExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<ITransactionHandler, TransactionCreatedEventHandler>();
        return services;
    }
}