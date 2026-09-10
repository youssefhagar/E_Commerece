using AutoMapper;
using E_Commerece.Application.Dtos;
using E_Commerece.Domain.Entites.Baskets;
using E_Commerece.Domain.Entites.Products;
using E_Commerece.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Profiles
{
    public class BasketProfile : Profile
    {

        public BasketProfile()
        {
           
            CreateMap<BasketItem, BasketItemDto>().ReverseMap();
            CreateMap<CustomerBasket, BasketDto>().ReverseMap();

        }

    }
}
