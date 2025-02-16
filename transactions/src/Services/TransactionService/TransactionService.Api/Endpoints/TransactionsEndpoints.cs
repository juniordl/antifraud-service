using MediatR;
using Microsoft.AspNetCore.Mvc;
using TransactionServices.Application.Transaction.Commands;
using TransactionServices.Application.Transaction.Dto;
using TransactionServices.Application.Transaction.Events;
using TransactionServices.Application.Transaction.Queries;

namespace TransactionServices.Endpoints;

public static class TransactionsEndpoints
{
    private const string ControllerName = "transactions";

    public static void MapTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var routeGroupBuilder = app.MapGroup($"/{ControllerName}")
            .WithTags("transactions")
            .WithOpenApi();
        
        routeGroupBuilder
            .MapPost("/", async ([FromServices] IMediator mediator, TransactionDto transaction) =>
            {
                return Results.Ok(await mediator.Send(new CreateTransactionCommandRequest(transaction)));
            })
            .Produces<CreateTransactionCommandResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
        
        routeGroupBuilder
            .MapGet("/{id}", async ([FromServices] IMediator mediator, Guid transactionId) =>
            {
                return Results.Ok(await mediator.Send(new GetTransactionQueryRequest(transactionId)));
            })
            .Produces<GetTransactionQueryResponse>()
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);
    }
}