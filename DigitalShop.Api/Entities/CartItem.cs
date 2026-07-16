using DigitalShop.DTOs;
using System.ComponentModel.DataAnnotations;

namespace DigitalShop.Entities
{
    public class CartItem
    {
        [Key]
        public Guid CartItemId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;
        public decimal Quantity { get; set; } = 1;
    }
}