using DigitalShop.Infrastructure.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Infrastructure.Entities.dbFilter
{
    public class ProductFilterOptions
    {
        private const int MaxPageSize = 100;

        public int PageIndex { get; set; }
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
