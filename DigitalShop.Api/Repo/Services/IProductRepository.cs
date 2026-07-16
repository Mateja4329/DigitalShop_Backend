using DigitalShop.DTOs.Product;
using DigitalShop.Entities;
using DigitalShop.Helpers;

namespace DigitalShop.Repo.Services
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<PaginatedList<Product>> GetAllProductsAsync(ProductQueryParameter queryParams);
        Task<Product?> GetProductByIdAsync(Guid productId);
        Task<Product> UpdateAsync(Product product);
        Task<List<Product>> DeleteAllProductsAsync();
        Task<Product?> DeleteProductByIdAsync(Guid productId);
    }
}
