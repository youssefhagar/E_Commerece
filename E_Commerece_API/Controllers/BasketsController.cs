using Azure;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerece.API.Controllers
{
    [Authorize]
    public class BasketsController : ApiBaseController
    {
        private readonly IBasketService basketService;

        public BasketsController(IBasketService basketService)
        {
            this.basketService = basketService;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BasketDto),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        
        public async Task<ActionResult<BasketDto>> GetBasket(string id , CancellationToken ct)
        {
            var basket = await basketService.GetBasketAsync(id, ct);
            return ToActionResult(basket);
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket, CancellationToken ct)
        {
            var result = await basketService.CreateOrUpdateBasketAsync(basket, null, ct);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var result = await basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }

    }
}
