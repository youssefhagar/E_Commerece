using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Contract
{
    public interface ICachRepository
    {

        Task<string?> GetAsync(string CachKey, CancellationToken ct);
        Task SetAsync(string CachKey, string CachValue, TimeSpan timeToLive , CancellationToken ct = default!);


    }
}
