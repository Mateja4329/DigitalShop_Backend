using DigitalShop.Application.DTOs.CartDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Application.Services.Interface
{
    public interface ICartService
    {
        Task<CartResponseDTO> AddCartApp(CartCreateDTO cartCreateDTO);
        Task<IEnumerable<CartResponseDTO>?> GetAllCartsApp(Guid userId);
        Task<CartResponseDTO?> GetCartByIdApp(Guid cartId);
        Task<CartResponseDTO?> UpdateCartApp(Guid cartId, CartCreateDTO request);
        Task<List<CartResponseDTO>> DeleteAllCartsApp(Guid userId);
        Task<CartResponseDTO?> DeleteCartByIdApp(Guid cartId);
    }
}
