using DigitalShop.Application.DTOs.CartDTO;
using DigitalShop.Application.Mappings;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Infrastructure.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
            var tokenUserIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(tokenUserIdString) || !Guid.TryParse(tokenUserIdString, out var userId))
            {
                return Unauthorized();
            }

            var validationResult = await validator.ValidateAsync(createDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var resultDto = await _cartService.AddCartApp(userId, createDTO);

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
            var tokenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(tokenUserId) || !Guid.TryParse(tokenUserId, out var tokenGuid) || tokenGuid != userId)
            {
                return Forbid("You cannot view another user's cart.");
            }

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
            var tokenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(tokenUserId) || !Guid.TryParse(tokenUserId, out var userId))
            {
                return Unauthorized(); // for safety
            }

            var cart = await _cartService.GetCartByIdApp(cartId, userId);
            if(cart == null)
            {
                return NotFound("Cart not found or you don't have permission to modify it.");
            }
            return Ok(cart);
        }

        // ==================== PUT ====================
        [HttpPut("{CartId:guid}/UpdateCart")]
        public async Task<ActionResult<CartResponseDTO>> UpdateCart(Guid cartId, CartCreateDTO request, [FromServices] IValidator<CartCreateDTO> validator)
        {
            var tokenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(tokenUserId) || !Guid.TryParse(tokenUserId, out var userId))
            {
                return Unauthorized();
            }

            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var savedCart = await _cartService.UpdateCartApp(cartId, userId, request);

            if (savedCart == null)
            {
                return NotFound("Cart not found or you don't have permission to modify it.");
            }

            return Ok(savedCart);
        }

        // ==================== DELETE ====================
        [HttpDelete("{userId:guid}/DeleteAllCarts")]
        public async Task<ActionResult<List<CartResponseDTO>>> DeleteAllCarts(Guid userId)
        {
            var tokenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(tokenUserId) || !Guid.TryParse(tokenUserId, out var tokenGuid) || tokenGuid != userId)
            {
                return Forbid("You cannot delete another user's cart.");
            }

            var remainingCarts = await _cartService.DeleteAllCartsApp(userId);
            if (remainingCarts == null || !remainingCarts.Any())
            {
                return NotFound("Cart not found");
            }
            return Ok(remainingCarts);
        }

        [HttpDelete("{cartId:guid}/DeleteCartById")]
        public async Task<ActionResult<CartResponseDTO>> DeleteCart(Guid cartId)
        {
            var tokenUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(tokenUserId) || !Guid.TryParse(tokenUserId, out Guid userId))
            {
                return Unauthorized();
            }

            var cart = await _cartService.DeleteCartByIdApp(cartId, userId);

            if (cart == null)
            {
                return NotFound("Cart not found or you don't have permission to modify it.");
            }
            return Ok(cart);
        }
    }
}
