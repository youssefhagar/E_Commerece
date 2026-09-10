using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos.OrderDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerece.API.Controllers
{
    [Authorize]
    public class OrderController(IOrderService orderService)
        : ApiBaseController
    {

        [HttpPost("create")]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(
            OrderToCreateDto order,
            CancellationToken cancellationToken)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            return ToActionResult(await orderService.CreateOrder(order, email, cancellationToken));

        }


    }
}
