using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Contracts
{
    public interface ICachService
    {
        Task<string?> GetAsync(string CachKey, CancellationToken ct = default);
        Task SetAsync(string CachKey, object CachValue, TimeSpan timeToLive , CancellationToken ct = default!);


    }
}
