using E_Commerece.Application.Common;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Params;
using E_Commerece.Domain.Shared;
using E_Commerece.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Contracts
{
    public interface IProductService
    {

        Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams param, CancellationToken ct = default);
        Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default);
        Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default);
        Task<Result<ProductDto>> GetProductByIdAsync(int productId, CancellationToken ct = default);

    }
}
