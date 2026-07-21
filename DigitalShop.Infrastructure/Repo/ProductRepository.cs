using DigitalShop.Infrastructure.Data;
using DigitalShop.Infrastructure.Entities;
using DigitalShop.Infrastructure.Entities.dbFilter;
using DigitalShop.Infrastructure.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalShop.Infrastructure.Repo
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext dataContext;
        public ProductRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        // POST ===================================
        public async Task<Product> AddProductAsync(Product product)
        {
            var result = await dataContext.Products.AddAsync(product);
            await dataContext.SaveChangesAsync();
            return result.Entity;
        }

        // GET ===================================
        // ALL -----------------------------------
        public async Task<(List<Product> Products, int TotalCount)> GetAllProductsAsync(ProductFilterOptions queryParams)
        {
            var query = dataContext.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(queryParams.SearchProductName))
            {
                query = query.Where(p => p.ProductName.ToLower().Contains(queryParams.SearchProductName.ToLower()));
            }

            if (queryParams.ProductCategory.HasValue)
            {
                query = query.Where(p => p.ProductCategory == queryParams.ProductCategory.Value);
            }

            if (queryParams.ProductCondition.HasValue)
            {
                query = query.Where(p => p.ProductCondition == queryParams.ProductCondition.Value);
            }

            if (queryParams.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= queryParams.MinPrice.Value);
            }

            if (queryParams.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= queryParams.MaxPrice.Value);
            }

            var products = await query
                .OrderBy(u => u.ProductId)
                .Skip((queryParams.PageIndex - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync(); // Database executes here

            var count = await query.CountAsync();

            return (products, count);
        }
        // ONE -----------------------------------
        public async Task<Product?> GetProductByIdAsync(Guid productId)
        {
            return await dataContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        // PUT ===================================
        public async Task<Product> UpdateAsync(Product product)
        {
            await dataContext.SaveChangesAsync();
            return product;
        }

        // DELETE ===================================
        public async Task<List<Product>> DeleteAllProductsAsync()
        {
            await dataContext.Products.ExecuteDeleteAsync();
            return await dataContext.Products.ToListAsync();
        }

        public async Task<Product?> DeleteProductByIdAsync(Guid productId)
        {
            var result = await dataContext.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

            if(result != null)
            {
                dataContext.Products.Remove(result);
                await dataContext.SaveChangesAsync();
                return result;
            }
            return null;
        }
    }
}
