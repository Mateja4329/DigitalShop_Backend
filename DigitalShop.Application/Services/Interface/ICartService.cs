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
        Task<CartResponseDTO> AddCartApp(Guid userId, CartCreateDTO cartCreateDTO);
        Task<IEnumerable<CartResponseDTO>?> GetAllCartsApp(Guid userId);
        Task<CartResponseDTO?> GetCartByIdApp(Guid cartId, Guid userId);
        Task<CartResponseDTO?> UpdateCartApp(Guid cartId, Guid userId, CartCreateDTO request);
        Task<List<CartResponseDTO>> DeleteAllCartsApp(Guid userId);
        Task<CartResponseDTO?> DeleteCartByIdApp(Guid cartId, Guid userId);
    }
}
