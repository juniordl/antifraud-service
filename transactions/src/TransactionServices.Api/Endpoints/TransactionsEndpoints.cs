namespace TransactionServices.Endpoints;

public static class TransactionsEndpoints
{
    private const string _controllerName = "schedules";

    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder routeGroupBuilder = app.MapGroup($"/{_controllerName}")
            .WithTags("transactions")
            .WithOpenApi();

        routeGroupBuilder.MapPost("/", (string body) =>
        {
            return Results.Ok(body);
        });
        
        routeGroupBuilder.MapGet("/{transactionId}", (Guid id) =>
        {
            return Results.Ok(id);
        });
    }
}