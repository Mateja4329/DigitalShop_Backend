using DigitalShop.Infrastructure.Entities;
using DigitalShop.Infrastructure.Entities.dbFilter;

namespace DigitalShop.Infrastructure.Repo.Interface
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<(List<Product> Products, int TotalCount)> GetAllProductsAsync(ProductFilterOptions queryParams);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product> UpdateAsync(Product product);
        Task<List<Product>> DeleteAllProductsAsync();
        Task<Product?> DeleteProductByIdAsync(Guid productId);
    }
}
