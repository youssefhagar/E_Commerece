using AutoMapper;
using E_Commerece.Application.Common;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Params;
using E_Commerece.Application.Specifications;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Products;
using E_Commerece.Domain.Shared;
using E_Commerece.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;

            this.mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand,int>().GetAllAsync(ct);
            var mappedBrands = mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(mappedBrands);
        }

        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams param, CancellationToken ct = default)
        {
            var spec = new ProductSpecififcation(param);
            var products = await _unitOfWork.GetRepository<Product,int>().GetAllAsync(spec,ct);

            var firstProduct = products.FirstOrDefault();
            Console.WriteLine($"PRODUCT ID FROM DATABASE: {firstProduct?.Id}");

            var mappedProducts = mapper.Map<IReadOnlyList<ProductDto>>(products);
            var firstDto = mappedProducts.FirstOrDefault();
            Console.WriteLine($"PRODUCT ID AFTER MAPPING: {firstDto?.Id}");
            var countspec = new ProductCountSpecification(param);
            var totalCount = await _unitOfWork.GetRepository<Product,int>().GetProductsCountAsync(countspec, ct);
            return Result<PaginatedResult<ProductDto>>.Ok(new PaginatedResult<ProductDto>
            (
                mappedProducts,
                 param.PageIndex,
                 param.PageSize,
                 totalCount
            //Count = await _unitOfWork.GetRepository<Product,int>().CountAsync(spec, ct)
            ));

        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var types = mapper.Map<IReadOnlyList<TypeDto>>( await _unitOfWork.GetRepository<ProductType,int>().GetAllAsync(ct));
            return Result<IReadOnlyList<TypeDto>>.Ok(types);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int productId, CancellationToken ct = default)
        {
            var spec = new ProductSpecififcation(productId);
            var product = await _unitOfWork.GetRepository<Product,int>().GetByIdASync(spec, ct);
            if (product == null)
                return Result<ProductDto>.Fail(Error.NotFound("ProductNotFound", $"Product with id {productId} not found."));

            var mappedProduct = mapper.Map<ProductDto>(product);
            return Result<ProductDto>.Ok(mappedProduct);

        }
    }
}
