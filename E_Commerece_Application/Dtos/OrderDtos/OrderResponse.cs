using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Dtos.OrderDtos
{
    public sealed record ProductItemOrderedResponse(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice);

}
