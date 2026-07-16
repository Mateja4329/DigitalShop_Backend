using DigitalShop.Entities.Enums;

namespace DigitalShop.DTOs.ProductDTO
{
    public class ProductCreateDTO
    {
        public string ProductName { get; set; } = string.Empty;
        public string ProductDescription { get; set; } = string.Empty;
        public Category ProductCategory { get; set; }
        public Condition ProductCondition { get; set; }
        public decimal Price { get; set; }
    }
}
