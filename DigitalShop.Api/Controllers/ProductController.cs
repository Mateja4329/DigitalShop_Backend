using DigitalShop.Data;
using DigitalShop.DTOs.CartDTO;
using DigitalShop.DTOs.Product;
using DigitalShop.DTOs.ProductDTO;
using DigitalShop.DTOs.User;
using DigitalShop.Entities;
using DigitalShop.Helpers;
using DigitalShop.Mappings;
using DigitalShop.Repo.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DigitalShop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        public ProductController(IProductRepository context)
        {
            _productRepository = context;
        }

        // ==================== POST ====================
        // Here we add a new product to the database and return the list of all products
        // Postman command: POST http://localhost:7255/api/product
        [HttpPost]
        public async Task<ActionResult<List<ProductResponseDTO>>> AddProduct(ProductCreateDTO createDTO, [FromServices] IValidator<ProductCreateDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(createDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            // mapping input (DTO -> DataBase)
            var newProduct = createDTO.ToProductEntity();

            // Save
            var savedProduct = await _productRepository.AddProductAsync(newProduct);

            // Map the output (DataBase -> DTO)
            return CreatedAtAction(nameof(AddProduct), new { id = savedProduct.ProductId }, savedProduct.ToProductResponseDto());
        }

        // ==================== GET ====================
        // Here we get all products from the database and return them
        // Postman command: GET http://localhost:7255/api/product
        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAllProducts([FromQuery] ProductQueryParameter queryParams, [FromServices] IValidator<ProductQueryParameter> validator)
        {
            var validationResult = await validator.ValidateAsync(queryParams);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var paginatedProducts = await _productRepository.GetAllProductsAsync(queryParams);
            var dtoList = paginatedProducts.Items.ToProductResponseDtoList();
            var paginatedDto = new PaginatedList<ProductResponseDTO>(dtoList, paginatedProducts.TotalCount, paginatedProducts.PageIndex, queryParams.PageSize);
            return Ok(new ApiResponse(true, "Products retrieved successfully", paginatedDto));
        }

        // Here we get a product by id from the database and return it
        // Postman command: GET http://localhost:7255/api/product/1
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProduct(Guid id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product.ToProductResponseDto());
        }

        // ==================== PUT ====================
        // Here we update a product by id in the database and return the list of all products
        // Postman command: PUT http://localhost:7255/api/product/1
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductCreateDTO>> UpdateProduct(Guid id, ProductCreateDTO request)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if(product == null)
            {
                return NotFound("Product not found");
            }

            product.ProductName = request.ProductName;
            product.ProductDescription = request.ProductDescription;
            product.ProductCategory = request.ProductCategory;
            product.ProductCondition = request.ProductCondition;
            product.Price = request.Price;

            var savedProduct = await _productRepository.UpdateAsync(product);
            
            return Ok(savedProduct.ToProductResponseDto());
        }

        // ==================== DELETE ====================
        // Here we delete all products from the database and return the list of all products
        // Postman command: DELETE http://localhost:7255/api/product
        [HttpDelete]
        public async Task<ActionResult<List<ProductResponseDTO>>> DeleteAllProduct()
        {
            var remainingProducts = await _productRepository.DeleteAllProductsAsync();
            return Ok(remainingProducts.ToProductResponseDtoList());
        }

        // Here we delete a product by id from the database and return the list of all products
        // Postman command: DELETE http://localhost:7255/api/product/1
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProductResponseDTO>> DeleteProduct(Guid id)
        {
            var product = await _productRepository.DeleteProductByIdAsync(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product.ToProductResponseDto());
        }
    }
}
