using DigitalShop.Infrastructure.Entities;

namespace DigitalShop.Infrastructure.Repo.Interface
{
    public interface ICartRepository
    {
        Task<CartItem> AddCartAsync(CartItem cart);
        Task<IEnumerable<CartItem>> GetAllCartsAsync(Guid userId);
        Task<CartItem?> GetCartByIdAsycn(Guid cartId);
        Task<CartItem?> UpdateCartAsync(Guid cartId, Guid newProductId, decimal newQuantity);
        Task<List<CartItem>> DeleteAllCartsAsync(Guid userId);
        Task<CartItem?> DeleteCartByIdAsync(Guid cartId);
    }
}
