using DigitalShop.Data;
using DigitalShop.DTOs.CartDTO;
using DigitalShop.Entities;
using DigitalShop.Repo.Services;
using Microsoft.EntityFrameworkCore;

namespace DigitalShop.Repo
{
    public class CartRepository : ICartRepository
    {
        public readonly DataContext dataContext;

        public CartRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        // POST ===================================
        public async Task<CartItem> AddCartAsync(CartItem cart)
        {
            // Check if this exact product is already in this specific user's cart
            // 'p' represents one row currently being checked in the database table
            var check = await dataContext.CartItems
                .Include(u => u.User)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(c => c.ProductId == cart.ProductId && c.UserId == cart.UserId);
            if (check != null)
            {
                check.Quantity += cart.Quantity;
                await dataContext.SaveChangesAsync();
                return check;
            }
            var result = await dataContext.CartItems.AddAsync(cart);
            await dataContext.SaveChangesAsync();
            return await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .FirstAsync(c => c.CartItemId == result.Entity.CartItemId);
        }

        // GET ===================================
        // ALL -----------------------------------
        public async Task<IEnumerable<CartItem>> GetAllCartAsync(Guid userId)
        {
            return await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }
        // ONE -----------------------------------
        public async Task<CartItem?> GetCartByIdAsycn(Guid userId, Guid productId)
        {
            return await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ProductId == productId);
        }

        // PUT ===================================
        public async Task<CartItem?> UpdateCartAsync(Guid userId, Guid oldProductId, CartCreateDTO request)
        {
            // Exception: If the user wants to update the original product
            if (oldProductId == request.ProductId)
            {
                var cart = await dataContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == oldProductId);

                if (cart == null) return null;

                cart.Quantity = request.Quantity;
                await dataContext.SaveChangesAsync();
                return await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .FirstAsync(p => p.CartItemId == cart.CartItemId);
            }
            // We check if the old product that we want to update exists
            var oldCartItem = await dataContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == oldProductId);
            if (oldCartItem == null) return null;

            oldCartItem.Quantity -= request.Quantity;
            if(oldCartItem.Quantity <= 0) dataContext.CartItems.Remove(oldCartItem);
            // Then we check if the product we want to update to exists in the users cart
            var newCartItem = await dataContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == request.ProductId);

            if (newCartItem != null)
            {
                newCartItem.Quantity += request.Quantity;
            }
            else
            {
                newCartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };
                await dataContext.CartItems.AddAsync(newCartItem);
            }
            await dataContext.SaveChangesAsync();
            return await dataContext.CartItems
                .Include(p => p.ProductId)
                .Include(u => u.User)
                .FirstAsync(c => c.CartItemId == newCartItem.CartItemId);
        }

        // DELETE ===================================
        // ALL -----------------------------------
        public async Task<List<CartItem>> DeleteAllCartsAsync(Guid userId)
        {
            var cartsToDelete = await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .Where(u => u.UserId == userId)
                .ToListAsync();

            if (cartsToDelete.Any())
            {
                dataContext.CartItems.RemoveRange(cartsToDelete);
                await dataContext.SaveChangesAsync();
            }

            return cartsToDelete;
        }

        // ONE -----------------------------------
        public async Task<CartItem> DeleteCartByIdAsync(CartItem cart)
        {
            dataContext.CartItems.Remove(cart);
            await dataContext.SaveChangesAsync();
            return cart;
        }
    }
}
