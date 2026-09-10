using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Dtos
{
    public sealed record PaymentClientSecretResponse(
    Guid OrderId,
    string PaymentIntentId,
    string ClientSecret,
    string Status,
    string PublishableKey);
}
