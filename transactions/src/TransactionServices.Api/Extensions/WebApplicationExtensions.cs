using TransactionServices.Endpoints;

namespace TransactionServices.Extensions;

public static class WebApplicationExtensions
{
    public static void MapEndpoints(this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("api/v1");
        routeGroupBuilder.MapTransactionEndpoints();
    }
}