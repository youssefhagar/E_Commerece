using E_Commerece.Domain.Errors;
using E_Commerece.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerece.Domain.Entites.Orders
{
    public class Order : BaseEntity<Guid>
    {
        public Guid UserId { get; private set; }
        public DateTime OrderDate { get;set; } = DateTime.Now;
        public string Email { get; set; } = default!;
        public OrderAddress Address { get; set; } = default!; // Owned Entity Not REaltion
        public decimal SubTotal { get; set; }
        public ICollection<OrderItem> Items { get; set; } = [];
        public OrderPaymentStatus PaymentStatu { get; set; } = OrderPaymentStatus.Pending;

        public DeliveryMethod DeliveryMethod { get; set; } = default!;
        public int DeliveryMethodId { get; set; }
        public OrderStatus Status { get; private set; }

        //Payment
        public string? PaymentIntentId { get; private set; } = default!;
        public DateTimeOffset? PaidAtUtc { get; private set; }

        private Order()
        {
        }

        public Order(Guid userId)
        {
            UserId = userId;
            Status = OrderStatus.Pending;
        }

        public Result AttachPaymentIntent(string paymentIntentId)
        {
            if (Status != OrderStatus.Pending)
                return Result.Fail(OrderErrors.InvalidPaymentState);

            if (string.IsNullOrWhiteSpace(paymentIntentId))
                return Result.Fail(OrderErrors.InvalidPaymentIntent);

            PaymentIntentId = paymentIntentId.Trim();
            //UpdatedAt = DateTimeOffset.UtcNow;
            return Result.Success();
        }
        public Result MarkAsPaid(string paymentIntentId)
        {
            if (Status == OrderStatus.Cancelled)
                return Result.Fail(OrderErrors.CannotPayCancelled);

            // Idempotent: already paid
            if (PaymentStatu == OrderPaymentStatus.Success
                && PaymentIntentId == paymentIntentId
                && PaidAtUtc is not null)
                return Result.Success();

            if (Status != OrderStatus.Pending)
                return Result.Fail(OrderErrors.InvalidPaymentState);

            if (!string.IsNullOrWhiteSpace(PaymentIntentId)
                && PaymentIntentId != paymentIntentId)
                return Result.Fail(OrderErrors.PaymentIntentMismatch);

            PaymentIntentId = paymentIntentId;
            PaidAtUtc = DateTimeOffset.UtcNow;

            // Payment succeeded
            PaymentStatu = OrderPaymentStatus.Success;

            // Order can now be processed
            Status = OrderStatus.Processing;

            return Result.Success();
        }

        //public Result MarkAsPaid(string paymentIntentId)
        //{
        //    if (Status == OrderStatus.Cancelled)
        //        return Result.Fail(OrderErrors.CannotPayCancelled);

        //    // Idempotent: already paid with same intent
        //    if (Status == OrderStatus.Processing
        //        && PaymentIntentId == paymentIntentId
        //        && PaidAtUtc is not null)
        //        return Result.Success();

        //    if (Status != OrderStatus.Pending)
        //        return Result.Fail(OrderErrors.InvalidPaymentState);

        //    if (!string.IsNullOrWhiteSpace(PaymentIntentId)
        //        && PaymentIntentId != paymentIntentId)
        //        return Result.Fail(OrderErrors.PaymentIntentMismatch);

        //    PaymentIntentId = paymentIntentId;
        //    PaidAtUtc = DateTimeOffset.UtcNow;
        //    Status = OrderStatus.Processing;
        //    //UpdatedAt = DateTimeOffset.UtcNow;
        //    return Result.Success();
        //}

    }

    public enum OrderPaymentStatus
    {
        Pending =0,
        Faild =1,
        Success=2,
    }
}
