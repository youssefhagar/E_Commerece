using E_Commerece.Domain.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Infrastructure.Repository
{
    public class CachRepository : ICachRepository
    {
        private readonly IDatabase _database;

        public CachRepository(IConnectionMultiplexer connection) {
            _database = connection.GetDatabase();
        }



        public async Task<string?> GetAsync(string CachKey, CancellationToken ct)
        {
            var value = await _database.StringGetAsync(CachKey);
            return value.IsNullOrEmpty ? null : value.ToString();
        }

        public async Task SetAsync(string CachKey, string CachValue, TimeSpan timeToLive = default, CancellationToken ct = default)
        {
            await _database.StringSetAsync(CachKey, CachValue, timeToLive);
        }
    }
}
