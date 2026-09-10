using E_Commerece.Domain.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Specifications
{
    internal class ProductsWithIdsSpec : BaseSpecification<Product, int>
    {
        public ProductsWithIdsSpec(IEnumerable<int> Ids) :base(P => Ids.Contains(P.Id))
        {
            
        }
    }
}
