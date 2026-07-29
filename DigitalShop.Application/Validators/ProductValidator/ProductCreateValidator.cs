using DigitalShop.Application.DTOs.ProductDTO;
using FluentValidation;

namespace DigitalShop.Application.Validators.ProductValidator
{
    public sealed class ProductCreateValidator : AbstractValidator<ProductCreateDTO>
    {
        public ProductCreateValidator()
        {
            RuleFor(p => p.ProductName)
                .NotEmpty().WithMessage("Product name is required")
                .MinimumLength(2).WithMessage("Product name must be at least 2 characters");

            RuleFor(p => p.ProductDescription)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");
        }
    }
}
