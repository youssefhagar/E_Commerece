using E_Commerece.Application.Common.Settings;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Orders;
using E_Commerece.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Payment.HandleStripeWebhook
{
    public sealed class HandleStripeWebhookCommandHandler(
        IUnitOfWork _unitOfWork,
        ILogger<HandleStripeWebhookCommandHandler> _logger,
        IOptions<StripSetting> options)
    : IRequestHandler<HandleStripeWebhookCommand, Result>
    {
        private readonly StripSetting _settings = options.Value;

        
        public async Task<Result> Handle(HandleStripeWebhookCommand request,CancellationToken cancellationToken)
        {
            var stripeEvent = EventUtility.ConstructEvent( request.Payload, request.Signature, _settings.WebHookSecretkey);

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                if (paymentIntent is null)
                {
                    _logger.LogError("PaymentIntent is null in the Stripe event.");
                    return Result.Fail(Error.Failure("PaymentIntent is null in the Stripe event."));
                }
                     

                // Get OrderId from Stripe Metadata
                if (!paymentIntent.Metadata.TryGetValue("OrderId", out var orderIdValue))
                {
                    _logger.LogError("OrderId is not in the Stripe Metadata.");
                    return Result.Fail(Error.Failure("OrderId is missing from PaymentIntent metadata."));
                }
                    
                if (!Guid.TryParse(orderIdValue, out var orderId))
                {
                    _logger.LogError("Invalid OrderId in PaymentIntent metadata.");
                    return Result.Fail(Error.Failure("Invalid OrderId in PaymentIntent metadata."));
                }
                  _logger.LogInformation("PaymentIntent succeeded for OrderId: {OrderId}", orderId);

                // Get the Order
                try
                {
                    var order = await _unitOfWork.GetRepository<Order, Guid>().GetByIdASync(orderId, cancellationToken);
                    if (order is null)
                    {
                        _logger.LogError("Order with Id {OrderId} not found. in unit of work get repo", orderId);
                        return Result.Fail(Error.Failure("Order not found."));

                    }
                    // Mark Order as Paid
                    var markAsPaidResult = order.MarkAsPaid(paymentIntent.Id);
                    if (markAsPaidResult.IsFailure)
                    {
                        _logger.LogError("Failed to mark Order with Id {OrderId} as paid. Error: {Error}", orderId, markAsPaidResult.Errors.FirstOrDefault()?.Description);
                        return markAsPaidResult;
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing the Stripe webhook for OrderId: {OrderId}", orderId);
                    throw;
                }

                
            }

            return  Result.Success();
        }
    }
}
