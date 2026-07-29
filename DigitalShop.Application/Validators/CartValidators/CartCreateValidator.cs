using DigitalShop.Application.DTOs.CartDTO;
using FluentValidation;

namespace DigitalShop.Application.Validators.CartValidators
{
    public sealed class CartCreateValidator : AbstractValidator<CartCreateDTO>
    {
        public CartCreateValidator()
        {
            RuleFor(c => c.ProductId)
                .NotEmpty().WithMessage("Product ID is required");

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("User ID is required");

            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1");
        }
    }
}
