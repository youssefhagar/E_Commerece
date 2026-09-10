using E_Commerece.Application.Common.Settings;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Specifications;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Orders;
using E_Commerece.Domain.Errors;
using E_Commerece.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Payment.CreateOrderPayment
{
    public sealed class CreateOrderPaymentCommandHandler(
    ICurrentUserService currentUser,
    //IRepository<Order> orderRepository,
    IUnitOfWork unitOfWork,
    IPaymentService paymentService,
    IOptions<StripSetting> stripeOptions)
    : IRequestHandler<CreateOrderPaymentCommand, Result<PaymentClientSecretResponse>>
    {
        public async Task<Result<PaymentClientSecretResponse>> Handle(
            CreateOrderPaymentCommand request,
            CancellationToken cancellationToken)
        {
            if (currentUser.UserId is null)
                return Result<PaymentClientSecretResponse>.Fail(OrderErrors.Unauthorized);

            var order = await unitOfWork.GetRepository<Order,Guid>().FirstOrDefaultAsync(
                new OrderByIdForUserSpecification(request.OrderId, currentUser.UserId.Value),
                cancellationToken);

            if (order is null)
                return Result<PaymentClientSecretResponse>.Fail(OrderErrors.NotFound);

            if (order.Status != OrderStatus.Pending)
                return Result<PaymentClientSecretResponse>.Fail(OrderErrors.InvalidPaymentState);

            var currency = stripeOptions.Value.Currency;
            var amountInSmallestUnit = (long)Math.Round(
                order.SubTotal * 100m, MidpointRounding.AwayFromZero);

            Result<PaymentIntentResult> intentResult;

            if (string.IsNullOrWhiteSpace(order.PaymentIntentId))
            {
                intentResult = await paymentService.CreatePaymentIntentAsync(
                    amountInSmallestUnit,
                    currency,
                    order.Id,
                    cancellationToken);
            }
            else
            {
                intentResult = await paymentService.UpdatePaymentIntentAsync(
                    order.PaymentIntentId,
                    amountInSmallestUnit,
                    order.Id,
                    cancellationToken);
            }

            if (intentResult.IsFailure)
                return Result<PaymentClientSecretResponse>.Fail(Error.Failure());

            var attach = order.AttachPaymentIntent(intentResult.Data.PaymentIntentId);
            if (attach.IsFailure)
                return Result<PaymentClientSecretResponse>.Fail(Error.Failure());

            unitOfWork.GetRepository<Order, Guid>().Update(order);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var value = intentResult.Data;
            return Result<PaymentClientSecretResponse>.Ok(
                new PaymentClientSecretResponse(
                    order.Id,
                    value.PaymentIntentId,
                    value.ClientSecret,
                    value.Status,
                    value.PublishableKey));
        }
    }
}
