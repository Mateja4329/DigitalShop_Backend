using DigitalShop.Application.DTOs.Product;
using DigitalShop.Application.DTOs.ProductDTO;
using DigitalShop.Application.Helpers;
using DigitalShop.Application.Mappings;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Infrastructure.Entities.dbFilter;
using DigitalShop.Infrastructure.Repo.Interface;

namespace DigitalShop.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        // POST ------------------------------------------------------------------------
        public async Task<ProductResponseDTO> AddProductApp(ProductCreateDTO createDTO)
        {
            var newProduct = createDTO.ToProductEntity();
            var savedProduct = await _repository.AddProductAsync(newProduct);
            return savedProduct.ToProductResponseDto();
        }

        // GET ------------------------------------------------------------------------
        public async Task<PaginatedList<ProductResponseDTO?>> GetAllProductsApp(ProductQueryParameter queryParams)
        {
            ProductFilterOptions ok = new ProductFilterOptions()
            {
                PageIndex = queryParams.PageIndex,
                PageSize = queryParams.PageSize,
                SearchProductName = queryParams.SearchProductName,
                ProductCategory = queryParams.ProductCategory,
                ProductCondition = queryParams.ProductCondition
            };
            var paginatedRequest = await _repository.GetAllProductsAsync(ok);

            var dtoList = paginatedRequest.Products
                .ToProductResponseDtoList()
                .Cast<ProductResponseDTO?>()
                .ToList();

            var paginatedDto = new PaginatedList<ProductResponseDTO?>(
                dtoList,
                paginatedRequest.TotalCount,
                queryParams.PageIndex,
                queryParams.PageSize
            );

            return paginatedDto;
        }
        public async Task<ProductResponseDTO?> GetProductByIdApp(Guid productId)
        {
            var product = await _repository.GetProductByIdAsync(productId);
            if (product == null) return null;

            return product.ToProductResponseDto();
        }

        // PUT ------------------------------------------------------------------------
        public async Task<ProductResponseDTO?> UpdateApp(Guid productId, ProductCreateDTO request)
        {
            var product = await _repository.GetProductByIdAsync(productId);
            if (product == null) return null;

            product.ProductName = request.ProductName;
            product.ProductDescription = request.ProductDescription;
            product.ProductCategory = request.ProductCategory;
            product.ProductCondition = request.ProductCondition;
            product.Price = request.Price;

            var savedProduct = await _repository.UpdateAsync(product);
            return savedProduct.ToProductResponseDto();
        }

        // DELETE ------------------------------------------------------------
        public async Task<List<ProductResponseDTO>> DeleteAllProductsApp()
        {
            var remainingProducts = await _repository.DeleteAllProductsAsync();
            return remainingProducts.ToProductResponseDtoList();
        }
        public async Task<ProductResponseDTO?> DeleteProductByIdApp(Guid productId)
        {
            var targetProduct = await _repository.DeleteProductByIdAsync(productId);
            if (targetProduct == null) return null;
            return targetProduct.ToProductResponseDto();
        }
    }
}
