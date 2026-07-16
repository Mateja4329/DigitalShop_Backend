using DigitalShop.DTOs.User;
using FluentValidation;

namespace DigitalShop.Validators
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
