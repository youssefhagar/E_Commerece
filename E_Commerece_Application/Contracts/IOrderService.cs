using E_Commerece.Application.Common;
using E_Commerece.Application.Dtos.OrderDtos;
using E_Commerece.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Contracts
{
    public interface IOrderService
    {

        Task<Result<OrderToReturnDto>> CreateOrder(
            OrderToCreateDto orderToCreate,
            string email,
            CancellationToken ct = default);


    }
}
