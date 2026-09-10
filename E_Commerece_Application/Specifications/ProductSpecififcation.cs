using E_Commerece.Application.Params;
using E_Commerece.Domain.Entites.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Specifications
{
    internal class ProductSpecififcation : BaseSpecification<Product, int>
    {
        public ProductSpecififcation(ProductQueryParams param) : base(p=>( !param.BrandId.HasValue || p.BrandId == param.BrandId) 
        && (!param.TypeId.HasValue || p.TypeId == param.TypeId) 
        && (string.IsNullOrEmpty(param.SearchValue) || p.Name.ToLower().Contains(param.SearchValue.ToLower())))
        {
            AddInclude(p=> p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch(param.Sort)
            {
                case ProductSortingOption.PriceAsc : AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOption.PriceDesc : AddOrderByDesc(p => p.Price);
                    break;
                case ProductSortingOption.NameAsc : AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOption.NameDesc :AddOrderByDesc(p => p.Name);
                    break;
                _: AddOrderBy(p => p.Name);
            };

            ApplyPaging(param.PageSize, param.PageIndex);

        }
        public ProductSpecififcation(int id) : base(p=>p.Id == id)
        {
            AddInclude(p=> p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
    }
}
