using E_Commerece.API.Attributes;
using E_Commerece.Application.Common;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Params;
using E_Commerece.Infrastructure.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerece.API.Controllers
{

    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        //GetAllProducts
        [HttpGet]
        [RedisCached]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams param, CancellationToken ct)
        {
            var products = await _productService.GetAllProductsAsync(param, ct);
            return ToActionResult(products);
        }

        //GetProductById
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id,CancellationToken ct)
        {
            var product = await _productService.GetProductByIdAsync(id,ct);
            return ToActionResult(product);
        }
        //GetBrands
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetBrands(CancellationToken ct)
        {
            var brands = await _productService.GetAllBrandsAsync(ct);
            return ToActionResult(brands);
        }
        //GetTypes
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetTypes(CancellationToken ct)
        {
            var types = await _productService.GetAllTypesAsync(ct);
            return ToActionResult(types);
        }
    }
}
