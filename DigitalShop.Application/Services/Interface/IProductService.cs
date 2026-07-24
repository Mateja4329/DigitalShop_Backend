using DigitalShop.Application.DTOs.Product;
using DigitalShop.Application.DTOs.ProductDTO;
using DigitalShop.Application.Helpers;
using DigitalShop.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Application.Services.Interface
{
    public interface IProductService
    {
        Task<ProductResponseDTO> AddProductApp(ProductCreateDTO createDTO);
        Task<PaginatedList<ProductResponseDTO>> GetAllProductsApp(ProductQueryParameter queryParams);
        Task<ProductResponseDTO?> GetProductByIdApp(Guid productId);
        Task<ProductResponseDTO?> UpdateApp(Guid productId, ProductCreateDTO request);
        Task<List<ProductResponseDTO>> DeleteAllProductsApp();
        Task<ProductResponseDTO?> DeleteProductByIdApp(Guid productId);
    }
}
