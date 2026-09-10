using E_Commerece.Application.Common;
using E_Commerece.Application.Common.Settings;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Specifications;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Orders;
using E_Commerece.Domain.Shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Service
{
    public class StripePaymentService(
        IOptions<StripSetting> stripeOptions,   
        IUnitOfWork unitOfWork,
        ILogger<StripePaymentService> logger)
        : IPaymentService
    {
        private readonly StripSetting _settings = stripeOptions.Value;

        public async Task<Result<PaymentIntentResult>> CreatePaymentIntentAsync(
        long amountInSmallestUnit,
        string currency,
        Guid orderId,
        CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_settings.Secretkey))
                return Result<PaymentIntentResult>.Fail(Error.Failure("Payment.Failuer","Fail To pay Please Try again"));

            if (amountInSmallestUnit < 1)
                return Result<PaymentIntentResult>.Fail(Error.Validation("Payment.Failuer", "Invalid payment amount"));

            StripeConfiguration.ApiKey = _settings.Secretkey;

            var service = new PaymentIntentService();
            var requestOptions = new RequestOptions
            {
                IdempotencyKey = $"payment_intent_order_{orderId:D}"
            };

            try
            {
                var paymentIntent = await service.CreateAsync(
                    new PaymentIntentCreateOptions
                    {
                        Amount = amountInSmallestUnit,
                        Currency = currency.ToLowerInvariant(),
                        PaymentMethodTypes = ["card"],
                        Metadata = new Dictionary<string, string>
                        {
                            ["OrderId"] = orderId.ToString("D")
                        }
                    },
                    requestOptions,
                    cancellationToken);

                logger.LogInformation(
                    "Created PaymentIntent {Id} for Order {OrderId}",
                    paymentIntent.Id, orderId);

                return ToResult(paymentIntent);
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe create failed for Order {OrderId}", orderId);
                return Result<PaymentIntentResult>.Fail(Error.Failure("Payment.Failuer", "Fail To pay Please Try again"));
            }
        }

        public async Task<Result<PaymentIntentResult>> UpdatePaymentIntentAsync(
            string paymentIntentId,
            long amountInSmallestUnit,
            Guid orderId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_settings.Secretkey)) 
                return Result<PaymentIntentResult>.Fail(Error.Failure("Payment.Failuer", "Fail To pay Please Try again"));

            if (string.IsNullOrWhiteSpace(paymentIntentId))
                return Result<PaymentIntentResult>.Fail(Error.Failure("Payment.Failuer", "Invalid payment intent ID"));

            if (amountInSmallestUnit < 1)
                return Result<PaymentIntentResult>.Fail(Error.Failure("Payment.Failuer", "Invalid payment amount"));

            StripeConfiguration.ApiKey = _settings.Secretkey;

            var service = new PaymentIntentService();
            var requestOptions = new RequestOptions
            {
                IdempotencyKey = $"payment_intent_order_{orderId:D}_update"
            };

            try
            {
                var paymentIntent = await service.UpdateAsync(
                    paymentIntentId,
                    new PaymentIntentUpdateOptions { Amount = amountInSmallestUnit },
                    requestOptions,
                    cancellationToken);

                logger.LogInformation(
                    "Updated PaymentIntent {Id} for Order {OrderId}",
                    paymentIntent.Id, orderId);

                return ToResult(paymentIntent);
            }
            catch (StripeException ex)
            {
                logger.LogError(ex, "Stripe update failed for Order {OrderId}", orderId);
                return Result<PaymentIntentResult>.Fail(Error.Failure("Payment.Failuer", "Fail To pay Please Try again"));
            }
        }

        public Task PaymentFailed(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            logger.LogWarning(
             $"Payment failed for PaymentIntent {paymentIntentId}. Order stays Pending.",
             paymentIntentId);
            return Task.CompletedTask;
        }

        public async Task PaymentSucceeded(string paymentIntentId, CancellationToken cancellationToken = default)
        {
            var order = await unitOfWork.GetRepository<Order,Guid>().FirstOrDefaultAsync(
            new OrderByPaymentIntentSpecification(paymentIntentId),
            cancellationToken);

            if (order is null)
            {
                logger.LogWarning(
                    "PaymentSucceeded: no order for PaymentIntent {Id}", paymentIntentId);
                return;
            }

            var paid = order.MarkAsPaid(paymentIntentId);
            if (paid.IsFailure)
            {
                logger.LogWarning(
                    "PaymentSucceeded MarkAsPaid failed for {OrderId}: {Error}",
                    order.Id, paid.Errors);
                return;
            }

            unitOfWork.GetRepository<Order, Guid>().Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            logger.LogInformation(
                "Payment succeeded for Order {OrderId}, Intent {IntentId}",
                order.Id, paymentIntentId);
        }

        



        private Result<PaymentIntentResult> ToResult(PaymentIntent paymentIntent)
        {
            if (string.IsNullOrWhiteSpace(paymentIntent.ClientSecret))
                return Result<PaymentIntentResult>.Fail(Error.Failure("Payment.Failuer", "Fail To pay Please Try again"));

            return Result<PaymentIntentResult>.Ok(new PaymentIntentResult(
                paymentIntent.Id,
                paymentIntent.ClientSecret,
                paymentIntent.Status,
                _settings.Publishablekey));
        }
    }
}
