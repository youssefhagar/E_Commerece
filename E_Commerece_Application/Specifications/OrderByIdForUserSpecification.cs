using E_Commerece.Domain.Entites.Identity;
using E_Commerece.Domain.Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Specifications
{
    internal class OrderByIdForUserSpecification : BaseSpecification<Order, Guid>
    {
        public OrderByIdForUserSpecification(Guid orderId, Guid userId)
            : base(a => a.Id == orderId && a.UserId == userId)
        {
            
        }
    }
}
