using E_Commerece.Application.Dtos;
using E_Commerece.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Payment.CreateOrderPayment
{
    public sealed record CreateOrderPaymentCommand(Guid OrderId)
    : IRequest<Result<PaymentClientSecretResponse>>;
}
