using DigitalShop.Infrastructure.Data;
using DigitalShop.Infrastructure.Entities;
using DigitalShop.Infrastructure.Repo.Interface;
using Microsoft.EntityFrameworkCore;

namespace DigitalShop.Infrastructure.Repo
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
        public async Task<IEnumerable<CartItem>> GetAllCartsAsync(Guid userId)
        {
            return await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .Where(u => u.UserId == userId)
                .ToListAsync();
        }
        // ONE -----------------------------------
        public async Task<CartItem?> GetCartByIdAsycn(Guid cartId, Guid userId)
        {
            return await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.CartItemId == cartId && c.UserId == userId);
        }

        // PUT ===================================
        public async Task<CartItem?> UpdateCartAsync(Guid cartId, Guid userId, Guid newProductId, decimal newQuantity)
        {
            // First we find the exact cart using CartId. This is an exception
            var existingCart = await dataContext.CartItems
                .Include(u => u.User)
                .Include(p => p.Product)
                .FirstOrDefaultAsync(c => c.CartItemId == cartId && c.UserId == userId);

            if (existingCart == null) return null;

            // SCENARIO A: User wants to change the the quantity of the same product
            if (existingCart.ProductId == newProductId)
            {
                existingCart.Quantity = newQuantity;
                await dataContext.SaveChangesAsync();
                return existingCart;
            }

            // SCENARIO B: User wants to switch the goods with something else
            // We have to check if the user already has that cart
            var productAlreadyInCart = await dataContext.CartItems
                .FirstOrDefaultAsync(c => c.UserId == existingCart.UserId && c.ProductId == newProductId);

            if (productAlreadyInCart != null)
            {
                productAlreadyInCart.Quantity += newQuantity;
                dataContext.CartItems.Remove(existingCart);
                await dataContext.SaveChangesAsync();

                return await dataContext.CartItems
                    .Include(p => p.Product)
                    .Include(u => u.User)
                    .FirstAsync(c => c.CartItemId == productAlreadyInCart.CartItemId);
            }
            else
            {
                // If the new product isn't in the cart, then we just swap the ids and quantity
                existingCart.ProductId = newProductId;
                existingCart.Quantity = newQuantity;
                await dataContext.SaveChangesAsync();

                return await dataContext.CartItems
                    .Include(p => p.Product)
                    .Include(u => u.User)
                    .FirstAsync(c => c.CartItemId == existingCart.CartItemId);
            }
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
        public async Task<CartItem?> DeleteCartByIdAsync(Guid cartId, Guid userId)
        {
            var cart = await dataContext.CartItems
                .Include(p => p.Product)
                .Include(u => u.User)
                .FirstOrDefaultAsync(c => c.CartItemId == cartId && c.UserId == userId);

            if (cart == null) return null;

            dataContext.CartItems.Remove(cart);
            await dataContext.SaveChangesAsync();
            return cart;
        }
    }
}
