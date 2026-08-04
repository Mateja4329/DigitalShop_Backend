using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Infrastructure.Entities.Report
{
    public class ReportMetrics
    {
        public string? MostSoldProduct { get; set; }
        public Dictionary<string, int> DistinctBuyersPerProduct { get; set; } = new();
        public List<string> NeverBoughtProducts { get; set; } = new();
        public List<string> UsersWhoBoughtSameProductMultipleTimes { get; set; } = new();
        public string? ProductWithMostDistinctBuyers { get; set; }
        public List<string> UsersWhoBoughtAllProducts { get; set; } = new();
        public List<string> ProductsBoughtByEveryUser { get; set; } = new();
    }
}
