using E_Commerece.Application.Payment.HandleStripeWebhook;
using MediatR;
using Stripe;

namespace ECommerce.API.Endpoints;

public static class PaymentEndpoints
{
    public static IEndpointRouteBuilder MapPaymentEndpoints(
        this IEndpointRouteBuilder endpoints)
        //ApiVersionSet apiVersionSet)
    {
        var group = endpoints
            .MapGroup("/api/payments")
            .WithTags("Payments")
            //.WithApiVersionSet(apiVersionSet)
            //.HasApiVersion(new ApiVersion(1, 0))
            ;

        group.MapPost("/webhook", async (
            HttpRequest request,
            ISender sender,
            CancellationToken ct) =>
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync(ct);
            var signature = request.Headers["Stripe-Signature"].ToString();

            try
            {
                var result = await sender.Send(
                new HandleStripeWebhookCommand(json, signature),ct);

                return result.IsFailure ? Results.BadRequest(): Results.Ok();
            }
            catch (StripeException)
            {

                throw;
            }
            

            
        })
        .AllowAnonymous()
        .WithSummary("Stripe webhook")
        .WithDescription("Verifies Stripe-Signature and marks orders paid on payment_intent.succeeded.");

        return endpoints;
    }
}