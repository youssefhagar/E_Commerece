using AutoMapper;
using E_Commerece.Application.Common;
using E_Commerece.Application.Contracts;
using E_Commerece.Application.Dtos;
using E_Commerece.Domain.Contract;
using E_Commerece.Domain.Entites.Baskets;
using E_Commerece.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Service
{
    public class BasketService(IBasketRepository _repository,IMapper _mapper) : IBasketService
    {
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, TimeSpan? timeToLive = null, CancellationToken ct = default)
        {
            var mappedBasket = _mapper.Map<CustomerBasket>(basket);
            var basketResult = await _repository.CreateOrUpdateBasketAsync(mappedBasket, timeToLive, ct);
            return basketResult != null ? Result<BasketDto>.Ok(_mapper.Map<BasketDto>(basketResult)) 
                : Result<BasketDto>.Fail(Error.Failure("BasketDelete.Failure", "Basket could not be created or updated."));

        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var result = await _repository.DeleteBasketAsync(basketId);
            return result ? Result<bool>.Ok(true) 
                : Result<bool>.Fail(Error.Failure("BasketDelete.Failure", "Basket could not be deleted."));

        }

        public async Task<Result<BasketDto>> GetBasketAsync(string id, CancellationToken ct)
        {
            var basket = await _repository.GetBasketAsync(id, ct);
            if(basket == null)
                return Result<BasketDto>.Fail(Error.NotFound( "Basket not found."));
            return Result<BasketDto>.Ok(_mapper.Map<BasketDto>(basket));
        }
    }
}
