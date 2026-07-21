using DigitalShop.Infrastructure.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace DigitalShop.Infrastructure.Entities
{
    // ======================================================================================
    // This class represents regular C# object (called POCO - Plain Old CLR Object)
    // that will be mapped to a database table by Entity Framework Core.
    // ======================================================================================
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public Category ProductCategory { get; set; }
        public Condition ProductCondition { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal Price { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
