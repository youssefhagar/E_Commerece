using E_Commerece.Domain.Entites.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Application.Specifications
{
    internal class OrderByPaymentIntentSpecification : BaseSpecification<Order, Guid>
    {
        public OrderByPaymentIntentSpecification(string paymentIntentId) 
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
            
        }
    }
}
