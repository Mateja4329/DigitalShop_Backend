using DigitalShop.Application.DTOs.User;
using DigitalShop.Application.Validators.UserValidators;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalShop.Tests.User.Validators
{
    [TestClass]
    public class UserRegistratorValidatorTest
    {
        private UserRegistratorValidator _validator = null!;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new UserRegistratorValidator();
        }

        [TestMethod]
        public async Task UserRegistrator_ShouldHaveErrors_WhenDataIsInvalid()
        {
            // Arrange
            var newUser = new UserCreateDTO()
            {
                FirstName = "", // empty string
                LastName = "A", // less than 2 characters
                Email = "not-an-email", // invalid email format
                Password = "123", // less than 6 characters
                PhoneNumber = "123" // less than 10 digits
            };

            // Act
            var result = await _validator.TestValidateAsync(newUser);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
            result.ShouldHaveValidationErrorFor(x => x.Email);
            result.ShouldHaveValidationErrorFor(x => x.Password);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
        }
    }
}
