using DigitalShop.Application.DTOs.CartDTO;
using DigitalShop.Application.Mappings;
using DigitalShop.Application.Services.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // ==================== POST ====================
        [HttpPost]
        public async Task<ActionResult<CartResponseDTO>> AddCart(CartCreateDTO createDTO, [FromServices] IValidator<CartCreateDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(createDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var resultDto = await _cartService.AddCartApp(createDTO);

            // Its not enough just to answer with 200 OK, but instead answer with 201 Created.
            return CreatedAtAction(
                nameof(GetCart), // Name of GET method in the controller
                new { cartId = resultDto.CartId }, // Parametters for the URL
                resultDto // Data which returns the response dto
            );
        }

        // ==================== GET ====================
        [HttpGet("user/{userId:guid}")]
        public async Task<ActionResult<IEnumerable<CartResponseDTO>>> GetAllCarts(Guid userId)
        {
            var carts = await _cartService.GetAllCartsApp(userId);
            if(carts == null || !carts.Any())
            {
                return NotFound("Cart not found");
            }
            return Ok(carts);
        }

        [HttpGet("{cartId:guid}")]
        public async Task<ActionResult<CartResponseDTO>> GetCart(Guid cartId)
        {
            var cart = await _cartService.GetCartByIdApp(cartId);
            if(cart == null)
            {
                return NotFound("Cart not found");
            }
            return Ok(cart);
        }

        // ==================== PUT ====================
        [HttpPut("{CartId:guid}/UpdateCart")]
        public async Task<ActionResult<CartCreateDTO>> UpdateCart(Guid cartId, CartCreateDTO request, [FromServices] IValidator<CartCreateDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var savedCart = await _cartService.UpdateCartApp(cartId, request);
            if (savedCart == null)
            {
                return NotFound("Original cart item not found");
            }

            return Ok(savedCart);
        }

        // ==================== DELETE ====================
        [HttpDelete("{userId:guid}/DeleteAllCarts")]
        public async Task<ActionResult<List<CartResponseDTO>>> DeleteAllCarts(Guid userId)
        {
            var remainingCarts = await _cartService.DeleteAllCartsApp(userId);
            if (remainingCarts == null || !remainingCarts.Any())
            {
                return NotFound("Cart not found");
            }
            return Ok(remainingCarts);
        }

        [HttpDelete("{CartId:guid}/DeleteCartById")]
        public async Task<ActionResult<CartCreateDTO>> DeleteCart(Guid cartId)
        {
            var cart = await _cartService.DeleteCartByIdApp(cartId);
            if( cart == null )
            {
                return NotFound("Cart not found");
            }
            return Ok(cart);
        }
    }
}
