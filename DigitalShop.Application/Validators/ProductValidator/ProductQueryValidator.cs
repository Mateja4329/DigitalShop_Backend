using DigitalShop.Application.DTOs.Product;
using FluentValidation;

namespace DigitalShop.Application.Validators.ProductValidator
{
    public sealed class ProductQueryValidator : AbstractValidator<ProductQueryParameter>
    {
        public ProductQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("Page index must be at least 1.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
                .WithMessage("Minimum price cannot be negative.");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(x => x.MinPrice).When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue)
                .WithMessage("Maximum price must be greater than or equal to minimum price.");
        }
    }
}