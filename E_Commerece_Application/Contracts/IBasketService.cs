using E_Commerece.Application.Common;
using E_Commerece.Application.Dtos;
using E_Commerece.Domain.Entites.Baskets;
using E_Commerece.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Contracts
{
    public interface IBasketService
    {

        Task<Result<BasketDto>> GetBasketAsync(string id, CancellationToken ct);
        Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan? timeToLive = null, CancellationToken ct = default!);
        Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default);



    }
}
