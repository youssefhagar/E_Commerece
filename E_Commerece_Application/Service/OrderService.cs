using AutoMapper;
using E_Commerece.Application.Common;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos.OrderDtos;
using E_Commerece.Application.Specifications;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Orders;
using E_Commerece.Domain.Entites.Products;
using E_Commerece.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Service
{
    public class OrderService(
        IUnitOfWork unitOfWork,
        IBasketRepository basketRepository,
        ICurrentUserService currentUserService,
        IMapper mapper) 
        : IOrderService
    {
        public async Task<Result<OrderToReturnDto>> CreateOrder(
            OrderToCreateDto orderToCreate,
            string email,
            CancellationToken ct = default)
        {
            // Get Basket From Basket Repository
            var basket = await basketRepository.GetBasketAsync(orderToCreate.BasketId, ct);
            if (basket == null)
                return Result<OrderToReturnDto>
                    .Fail(Error.NotFound("Basket.NotFound", "There is No Basket Found "));

            if(!basket.Items.Any())
                return Result<OrderToReturnDto>
                    .Fail(Error.NotFound("Basket Empty","Can't Create Order With Empty Basket"));

            //Get Delivary Method From UnitOfWork then Validated price
            var delivaryMethod =
                await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetByIdASync(orderToCreate.DelivaryMethod, ct);
             if(delivaryMethod == null )
                return Result<OrderToReturnDto>
                    .Fail(Error.NotFound("Delivary Method Not Found", "Delivary Method Not Found"));

            // Get Products
            var productIds = basket.Items.Select(P => P.Id).ToHashSet();
            var products =
                (await unitOfWork.GetRepository<Product, int>()
                .GetAllAsync(new ProductsWithIdsSpec(productIds), ct)).ToDictionary(p => p.Id);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                if(!products.TryGetValue(item.Id, out var product))
                    return Result<OrderToReturnDto>
                        .Fail(Error.NotFound());
                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    PictureUrl = product.PictureUrl,
                    Price = product.Price,
                    ProductName = product.Name,
                    Quantity = item.Quantity
                });

            }

            var address = mapper.Map<OrderAddress>(orderToCreate.Address);// Miss Mapping 
            var suptotal = orderItems.Sum(p=>p.Price*p.Quantity);

            if (currentUserService.UserId is null)
            {
                return Result<OrderToReturnDto>
                    .Fail(Error.Unauthorized());
            }

            var order = new Order(currentUserService.UserId.Value)
            {
                //UserId = currentUserService.UserId,
                DeliveryMethod = delivaryMethod,
                Items = orderItems,
                Email = email, // TODO : Check if User Email Exist in DB
                SubTotal = suptotal,
                Address = address,
                //Status = OrderStatus.Pending,
                //PaymentIntentId = "Test"
            };
            

            unitOfWork.GetRepository<Order,Guid>().Add(order);

            var result = await unitOfWork.SaveChangesAsync();

            if(result <0)
                return Result<OrderToReturnDto>
                    .Fail(Error.Failure());

            await basketRepository.DeleteBasketAsync(basket.Id);
            var mappedOrder = mapper.Map<OrderToReturnDto>(order);
            return Result<OrderToReturnDto>.Ok(mappedOrder);
        }
    }
}
