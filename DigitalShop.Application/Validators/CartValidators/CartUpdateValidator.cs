using FluentValidation;
using DigitalShop.Application.DTOs.Cart;

namespace DigitalShop.Application.Validators.CartValidators
{
    public sealed class CartUpdateValidator : AbstractValidator<CartUpdateDTO>
    {
        public CartUpdateValidator()
        {
            RuleFor(c => c.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1");
        }
    }
}
