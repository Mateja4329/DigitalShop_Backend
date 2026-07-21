using DigitalShop.Infrastructure.Entities;
using DigitalShop.Application.DTOs.Order;

namespace DigitalShop.Application.DTOs.Order
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDTO> Items { get; set; } = new List<OrderItemDTO>();
        public decimal TotalAmount { get; set; }
    }
}
