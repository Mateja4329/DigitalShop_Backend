using DigitalShop.Application.DTOs.CartDTO;
using DigitalShop.Application.Mappings;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Infrastructure.Repo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        // POST ----------------------------------------------------------------
        public async Task<CartResponseDTO> AddCartApp(Guid userId, CartCreateDTO cartCreateDTO)
        {
            var newCart = cartCreateDTO.ToCartEntity(userId);

            var savedCartEntity = await _repository.AddCartAsync(newCart);
            return savedCartEntity.ToCartResponseDto();
        }
        // GET ------------------------------------------------------------------
        public async Task<IEnumerable<CartResponseDTO>?> GetAllCartsApp(Guid userId)
        {
            var carts = await _repository.GetAllCartsAsync(userId);
            if (carts == null || !carts.Any()) return null;
            return carts.ToList().ToCartResponseDtoList();
        }
        public async Task<CartResponseDTO?> GetCartByIdApp(Guid cartId, Guid userId)
        {
            var cart = await _repository.GetCartByIdAsycn(cartId, userId);
            return cart?.ToCartResponseDto();
        }
        // PUT ---------------------------------------------------------------------
        public async Task<CartResponseDTO?> UpdateCartApp(Guid cartId, Guid userId, CartCreateDTO request)
        {
            var cart = await _repository.UpdateCartAsync(cartId, userId, request.ProductId, request.Quantity);
            return cart?.ToCartResponseDto();
        }
        // DELETE -------------------------------------------------------------------
        public async Task<List<CartResponseDTO>> DeleteAllCartsApp(Guid userId)
        {
            return (await _repository.DeleteAllCartsAsync(userId)).ToCartResponseDtoList();
        }

        public async Task<CartResponseDTO?> DeleteCartByIdApp(Guid cartId, Guid userId)
        {
            return (await _repository.DeleteCartByIdAsync(cartId, userId))?.ToCartResponseDto();
            // this is a Null-Conditional Operator (?.)
        }
    }
}
