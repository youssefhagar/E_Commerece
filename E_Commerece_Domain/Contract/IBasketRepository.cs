using E_Commerece.Domain.Entites.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Contract
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string id,CancellationToken ct);
        Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken ct = default!);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
