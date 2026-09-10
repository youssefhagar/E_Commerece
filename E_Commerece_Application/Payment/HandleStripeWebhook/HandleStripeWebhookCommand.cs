using E_Commerece.Domain.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Payment.HandleStripeWebhook
{
    public sealed record HandleStripeWebhookCommand(
    string Payload,
    string Signature) : IRequest<Result>;
}
