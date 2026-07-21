using DigitalShop.Application.DTOs.User;
using FluentValidation;

namespace DigitalShop.Application.Validators
{
    public class UserQueryValidator : AbstractValidator<UserQueryParameters>
    {
        public UserQueryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("Page index must be at least 1.");
        }
    }
}
