using E_Commerece.Application.Contracts;
using E_Commerece.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerece.Application.Service
{
    public class CachService : ICachService
    {
        private readonly ICachRepository repository;

        public CachService(ICachRepository repository)
        {
            this.repository = repository;
        }
        public async Task<string?> GetAsync(string CachKey, CancellationToken ct = default)
        {
            return await repository.GetAsync(CachKey, ct);
        }

        public Task SetAsync(string CachKey, object CachValue, TimeSpan timeToLive, CancellationToken ct = default)
        {
            var json = JsonSerializer.Serialize(CachValue, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
            return repository.SetAsync(CachKey, json, timeToLive, ct);
        }
    }
}
