using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Dtos.OrderDtos
{
    public class OrderToCreateDto
    {
        public string BasketId { get; set; } = default!;
        public int DelivaryMethod { get; set; }
        public AddressDto Address { get; set; } = default!;
    }
}
