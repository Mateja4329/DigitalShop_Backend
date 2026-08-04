using DigitalShop.Infrastructure.Data;
using DigitalShop.Infrastructure.Entities.Report;
using DigitalShop.Infrastructure.Repo.Interface.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalShop.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace DigitalShop.Infrastructure.Repo
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _dataContext;

        public ReportRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<ReportMetrics> GenerateReportAsync()
        {
            var metrics = new ReportMetrics();

            var cartItems = await _dataContext.CartItems
                .Include(c => c.Product)
                .Include(c => c.User)
                .ToListAsync();

            var products = await _dataContext.Products.ToListAsync();
            var users = await _dataContext.Users.ToListAsync();

            // 1. Which product is the most sold?
            metrics.MostSoldProduct = cartItems
                .GroupBy(c => c.Product.ProductName)
                .OrderByDescending(g => g.Sum(c => c.Quantity))
                .Select(g => g.Key)
                .FirstOrDefault();

            // 2. How many different users purchased each product?
            metrics.DistinctBuyersPerProduct = cartItems
                .GroupBy(c => c.Product.ProductName)
                .ToDictionary(g => g.Key, g => g.Select(c => c.UserId).Distinct().Count());

            // 3. Which product was never purchased by any user?
            var boughtProductIds = cartItems.Select(c => c.ProductId).Distinct();
            metrics.NeverBoughtProducts = products
                .Where(p => !boughtProductIds.Contains(p.ProductId))
                .Select(p => p.ProductName)
                .ToList();

            // 4. Which users purchased the same product multiple times?
            metrics.UsersWhoBoughtSameProductMultipleTimes = cartItems
                .Where(c => c.Quantity > 1)
                .Select(c => $"{c.User.FirstName} {c.User.LastName}")
                .Distinct()
                .ToList();

            // 5. Which product has the most unique customers?
            metrics.ProductWithMostDistinctBuyers = cartItems
                .GroupBy(c => c.Product.ProductName)
                .OrderByDescending(g => g.Select(c => c.UserId).Distinct().Count())
                .Select(g => g.Key)
                .FirstOrDefault();

            // 6. Which users bought all the products?
            var totalProducts = products.Count;
            if (totalProducts > 0)
            {
                metrics.UsersWhoBoughtAllProducts = cartItems
                    .GroupBy(c => new { c.UserId, c.User.FirstName, c.User.LastName })
                    .Where(g => g.Select(c => c.ProductId).Distinct().Count() == totalProducts)
                    .Select(g => $"{g.Key.FirstName} {g.Key.LastName}")
                    .ToList();
            }

            // 7. What are the products that every user bought?
            var totalUsers = users.Count;
            if (totalUsers > 0)
            {
                metrics.ProductsBoughtByEveryUser = cartItems
                    .GroupBy(c => c.Product.ProductName)
                    .Where(g => g.Select(c => c.UserId).Distinct().Count() == totalUsers)
                    .Select(g => g.Key)
                    .ToList();
            }

            return metrics;
        }
    }
}