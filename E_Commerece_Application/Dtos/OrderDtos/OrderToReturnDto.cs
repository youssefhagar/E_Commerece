using E_Commerece.Domain.Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Dtos.OrderDtos
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public string Email { get; set; } = default!;
        public AddressDto Address { get; set; } = default!; // Owned Entity Not REaltion
        
        public ICollection<OrderItemDtO> Items { get; set; } = [];
        
        public string DeliveryMethod { get; set; } = default!;

        public decimal SubTotal { get; set; }
        public decimal DelivaryCost { get; set; }
        public string PaymentStatu { get; set; } = default!;

        //Payment
        public string PaymentIntentId { get; set; } = default!;
        public decimal Total { get; set; }
    }
}
