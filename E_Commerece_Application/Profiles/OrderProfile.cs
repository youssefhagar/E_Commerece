using AutoMapper;
using E_Commerece.Application.Dtos;
using E_Commerece.Application.Dtos.OrderDtos;
using E_Commerece.Domain.Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Profiles
{
    public class OrderProfile : Profile
    {

        public OrderProfile()
        {
            CreateMap<AddressDto,OrderAddress>().ReverseMap();
            CreateMap<OrderItem, OrderItemDtO>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, opt => opt.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DelivaryCost, opt => opt.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.Total, opt => opt.MapFrom(s => s.DeliveryMethod.Price + s.SubTotal));

            CreateMap<DeliveryMethod, DeliveryMethodDto>();
                


        }

    }
}
