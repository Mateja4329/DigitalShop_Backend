using DigitalShop.Infrastructure.Entities;

namespace DigitalShop.Infrastructure.Repo.Interface
{
    public interface ICartRepository
    {
        Task<CartItem> AddCartAsync(CartItem cart);
        Task<IEnumerable<CartItem>> GetAllCartAsync(Guid userId);
        Task<CartItem?> GetCartByIdAsycn(Guid userId, Guid productId);
        Task<CartItem?> UpdateCartAsync(Guid userId, Guid oldProductId, CartCreateDTO request);
        Task<List<CartItem>> DeleteAllCartsAsync(Guid userId);
        Task<CartItem> DeleteCartByIdAsync(CartItem cart);
    }
}
