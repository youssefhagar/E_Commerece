namespace E_Commerece.Domain.Entites.Orders
{
    public class OrderItem : BaseEntity<int>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public Guid OrderId { get; set; }

    }
}