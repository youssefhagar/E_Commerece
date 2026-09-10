using E_Commerece.Application.Params;
using E_Commerece.Domain.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Specifications
{
    internal class ProductCountSpecification : BaseSpecification<Product, int>
    {

        public ProductCountSpecification(ProductQueryParams param) : base(p => (!param.BrandId.HasValue || p.BrandId == param.BrandId)
        && (!param.TypeId.HasValue || p.TypeId == param.TypeId)
        && (string.IsNullOrEmpty(param.SearchValue) || p.Name.ToLower().Contains(param.SearchValue.ToLower())))
        {
            
        }


    }
}
