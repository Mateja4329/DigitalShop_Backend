using DigitalShop.Application.DTOs.Product;
using DigitalShop.Application.DTOs.ProductDTO;
using DigitalShop.Application.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DigitalShop.Application.Services.Interface;

namespace DigitalShop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // ==================== POST ====================
        // Here we add a new product to the database and return the list of all products
        // Postman command: POST http://localhost:7255/api/product
        [Authorize(Roles = "Admin")]
        [HttpPost("AddProduct")]
        public async Task<ActionResult<List<ProductResponseDTO>>> AddProduct(ProductCreateDTO createDTO, [FromServices] IValidator<ProductCreateDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(createDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            // mapping input (DTO -> DataBase)
            // var newProduct = createDTO.ToProductEntity();

            // Save
            var savedProduct = await _productService.AddProductApp(createDTO);

            // Map the output (DataBase -> DTO)
            return CreatedAtAction(nameof(GetProductById), new { id = savedProduct.ProductId }, savedProduct);
        }

        // ==================== GET ====================
        // Here we get all products from the database and return them
        // Postman command: GET http://localhost:7255/api/product
        [AllowAnonymous]
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryParameter queryParams, [FromServices] IValidator<ProductQueryParameter> validator)
        {
            var validationResult = await validator.ValidateAsync(queryParams);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var paginatedProducts = await _productService.GetAllProductsApp(queryParams);
            
            return Ok(new ApiResponse(true, "Products retrieved successfully", paginatedProducts));
        }

        // Here we get a product by id from the database and return it
        // Postman command: GET http://localhost:7255/api/product/1
        [AllowAnonymous]
        [HttpGet("{id:guid}/GetProductById")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdApp(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product);
        }

        // ==================== PUT ====================
        // Here we update a product by id in the database and return the list of all products
        // Postman command: PUT http://localhost:7255/api/product/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/UpdateProduct")]
        public async Task<ActionResult<ProductCreateDTO>> UpdateProduct(Guid id, ProductCreateDTO request)
        {
            var product = await _productService.UpdateApp(id, request);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            
            return Ok(product);
        }

        // ==================== DELETE ====================
        // Here we delete all products from the database and return the list of all products
        // Postman command: DELETE http://localhost:7255/api/product
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteAllProducts")]
        public async Task<ActionResult<List<ProductResponseDTO>>> DeleteAllProducts()
        {
            var remainingProducts = await _productService.DeleteAllProductsApp();
            return Ok(remainingProducts);
        }

        // Here we delete a product by id from the database and return the list of all products
        // Postman command: DELETE http://localhost:7255/api/product/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}/DeleteOneProduct")]
        public async Task<ActionResult<ProductResponseDTO>> DeleteProductById(Guid id)
        {
            var product = await _productService.DeleteProductByIdApp(id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }
    }
}
