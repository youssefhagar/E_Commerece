namespace E_Commerece.Application.Dtos.OrderDtos
{
    public class OrderItemDtO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}