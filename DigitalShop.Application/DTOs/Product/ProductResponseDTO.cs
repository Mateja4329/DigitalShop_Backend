using DigitalShop.Infrastructure.Entities.Enums;

namespace DigitalShop.Application.DTOs.ProductDTO
{
    public class ProductResponseDTO
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public Category ProductCategory { get; set; }
        public Condition ProductCondition { get; set; }
        public decimal Price { get; set; }
    }
}
