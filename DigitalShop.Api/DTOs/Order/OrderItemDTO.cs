using DigitalShop.Entities.Enums;

namespace DigitalShop.DTOs.Order
{
    public class OrderItemDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public OrderStatus Status {  get; set; }
        public decimal PriceAtTimeOfOrder { get; set; }
    }
}
