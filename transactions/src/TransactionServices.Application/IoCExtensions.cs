using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using TransactionServices.Application.Interfaces;
using TransactionServices.Application.Transaction.Events;

namespace TransactionServices.Application;

public static class IoCExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddScoped<ITransactionHandler, TransactionEvaluatedEventHandler>();
        return services;
    }
}