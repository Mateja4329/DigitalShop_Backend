using DigitalShop.Entities;

namespace DigitalShop.DTOs.Order
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
        public decimal TotalAmount { get; set; }
    }
}
