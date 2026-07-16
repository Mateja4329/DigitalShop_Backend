using DigitalShop.DTOs.Cart;
using DigitalShop.DTOs.CartDTO;
using DigitalShop.DTOs.User;
using DigitalShop.Entities;
using DigitalShop.Mappings;
using DigitalShop.Repo.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DigitalShop.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // ==================== POST ====================
        [HttpPost]
        public async Task<ActionResult<List<CartResponseDTO>>> AddCart(CartCreateDTO createDTO, [FromServices] IValidator<CartCreateDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(createDTO);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var newCart = createDTO.ToCartEntity();

            var savedCart = await _cartRepository.AddCartAsync(newCart);

            return CreatedAtAction(nameof(GetCart), new { userId = savedCart.UserId, productId = savedCart.ProductId }, savedCart.ToCartResponseDto());
        }

        // ==================== GET ====================
        [HttpGet("{userId:guid}")]
        public async Task<ActionResult<List<CartResponseDTO>>> GetAllCarts(Guid userId)
        {
            var carts = await _cartRepository.GetAllCartAsync(userId);
            if(carts == null || !carts.Any())
            {
                return NotFound("Cart not found");
            }
            return Ok(carts.ToList().ToCartResponseDtoList());
        }

        [HttpGet]
        public async Task<ActionResult<CartResponseDTO>> GetCart(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetCartByIdAsycn(userId, productId);
            if(cart == null)
            {
                return NotFound("Cart not found");
            }
            return Ok(cart.ToCartResponseDto());
        }

        // ==================== PUT ====================
        [HttpPut("{UserId:guid}/{ProductId:guid}")]
        public async Task<ActionResult<CartCreateDTO>> UpdateCart(Guid UserId, Guid ProductId, CartCreateDTO request, [FromServices] IValidator<CartCreateDTO> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(new ValidationProblemDetails(validationResult.ToDictionary()));
            }

            var savedCart = await _cartRepository.UpdateCartAsync(UserId, ProductId, request);
            if (savedCart == null)
            {
                return NotFound("Original cart item not found");
            }

            return Ok(savedCart.ToCartResponseDto());
        }

        // ==================== DELETE ====================
        [HttpDelete("{userId:guid}")]
        public async Task<ActionResult<List<CartResponseDTO>>> DeleteAllCarts(Guid userId)
        {
            var remainingCarts = await _cartRepository.DeleteAllCartsAsync(userId);
            if (remainingCarts == null || !remainingCarts.Any())
            {
                return NotFound("Cart not found");
            }
            return Ok(remainingCarts.ToCartResponseDtoList());
        }

        [HttpDelete("{UserId:guid}/{ProductId:guid}")]
        public async Task<ActionResult<CartCreateDTO>> DeleteCart(Guid userId, Guid productId)
        {
            var cart = await _cartRepository.GetCartByIdAsycn(userId, productId);
            if( cart == null )
            {
                return NotFound("Cart not found");
            }
            var savedCart = await _cartRepository.DeleteCartByIdAsync(cart);

            return Ok(savedCart.ToCartResponseDto());
        }
    }
}
