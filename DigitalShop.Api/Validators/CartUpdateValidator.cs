using FluentValidation;
using DigitalShop.DTOs.CartDTO;
using DigitalShop.DTOs.Cart;

namespace DigitalShop.Validators
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
