using DigitalShop.Application.DTOs.User;
using FluentValidation;

namespace DigitalShop.Application.Validators
{
    public sealed class UserRegistratorValidator : AbstractValidator<UserCreateDTO>
    {
        public UserRegistratorValidator()
        {
            RuleFor(newUser => newUser.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .Length(2, 50).WithMessage("First name must be between 2 and 50 characters");

            RuleFor(newUser => newUser.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters");

            RuleFor(newUser => newUser.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(newUser => newUser.Password)
                .NotEmpty().WithMessage("Password is required")
                .Length(6, 100).WithMessage("Password must be between 6 and 100 characters");

            RuleFor(newUser => newUser.PhoneNumber)
                .NotEmpty()
                .Matches(@"^\d{10}$")
                .WithMessage("Phone number is required");
        }
    }
}
