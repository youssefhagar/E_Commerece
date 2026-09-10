using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Contract
{
    public interface IDataSeeder
    {

        Task SeedDataAsync(CancellationToken ct = default);

    }
}
