using E_Commerece.Application.Common;
using E_Commerece.Application.Dtos;
//using E_Commerece.Application.Features.Orders.Payment.CreateOrderPayment;
using E_Commerece.Application.Payment.CreateOrderPayment;
using E_Commerece.Domain.Shared;
using MediatR;
using System.Net;

namespace E_Commerece.API.Features.Orders.Payment;

public static class CreateOrderPaymentEndpoint
{
    public static IEndpointRouteBuilder MapCreateOrderPaymentEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
           .MapGroup("/api/orders")
           .WithTags("Orders");
        //group.MapPost(
        //    "/{id:guid}/pay",
        //    async (
        //        Guid id,
        //        ISender sender,
        //        HttpContext httpContext,
        //        CancellationToken ct) =>
        //    {
        //        var result = await sender.Send(
        //            new CreateOrderPaymentCommand(id),
        //            ct);

        //        return result;
        //    })
        //    .WithSummary(
        //        "Create Stripe PaymentIntent for order (or update amount if one already exists)")
        //    .WithDescription(
        //        "Uses order.Total (server-side). Calls CreatePaymentIntent when none exists; otherwise UpdatePaymentIntent. Returns clientSecret for Stripe.js.")
        //    .Produces<Result<PaymentClientSecretResponse>>(
        //        StatusCodes.Status200OK)
        //    .ProducesProblem(
        //        StatusCodes.Status400BadRequest)
        //    .ProducesProblem(
        //        StatusCodes.Status404NotFound)
        //    .ProducesProblem(
        //        StatusCodes.Status409Conflict);
        group.MapPost(
    "/{id:guid}/pay",
    async (
        Guid id,
        ISender sender,
        CancellationToken ct) =>
    {
        var result = await sender.Send(
            new CreateOrderPaymentCommand(id),
            ct);

        if (result.IsSuccess)
            return Results.Ok(result.Data);

        return Results.BadRequest(result.Errors);
    })
    .WithSummary(
        "Create Stripe PaymentIntent for order (or update amount if one already exists)")
    .WithDescription(
        "Uses order.Total (server-side). Calls CreatePaymentIntent when none exists; otherwise UpdatePaymentIntent. Returns clientSecret for Stripe.js.")
    .Produces<PaymentClientSecretResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest)
    .Produces(StatusCodes.Status404NotFound)
    .Produces(StatusCodes.Status409Conflict);

        return group;
    }
}