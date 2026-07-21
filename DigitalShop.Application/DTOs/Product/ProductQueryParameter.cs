using DigitalShop.Infrastructure.Entities.Enums;

namespace DigitalShop.Application.DTOs.Product
{
    public class ProductQueryParameter
    {
        private const int MaxPageSize = 100;

        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        public string? SearchProductName { get; set; }
        public Category? ProductCategory { get; set; }
        public Condition? ProductCondition { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? MinPrice { get; set; }
    }
}
