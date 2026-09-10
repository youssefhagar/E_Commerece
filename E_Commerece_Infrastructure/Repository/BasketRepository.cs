using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            database = connection.GetDatabase();
        }

        public async Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken ct = default!)
        {
           var json = JsonSerializer.Serialize(basket);
           var success = await database.StringSetAsync(basket.Id, json, timeToLive ?? TimeSpan.FromDays(30));
            return success ? basket : null;
        }


        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket> GetBasketAsync(string id, CancellationToken ct)
        {
            var basket =  await database.StringGetAsync(id);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonSerializer.Deserialize<CustomerBasket>(basket.ToString());

        }
    }
}
