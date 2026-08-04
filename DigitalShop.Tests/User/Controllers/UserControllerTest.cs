using DigitalShop.Controllers;
using DigitalShop.Application.DTOs.User;
using DigitalShop.Application.Validators.UserValidators;
using DigitalShop.Application.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalShop.Tests.User.Controllers
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController _controller = null!;
        private UserRegistratorValidator _validator = null!;
        private IUserService _service = null!;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new UserRegistratorValidator();
            _controller = new UserController(_service);
        }

        [TestMethod]
        public async Task RegisterUser_WhenValidationFails_ShouldReturnValidationProblem()
        {
            // Arrange
            var invalidUser = new UserCreateDTO()
            {
                FirstName = "",
                LastName = "Glisovic",
                Email = "test@example.com",
                Password = "Password123!",
                PhoneNumber = "0601234567"
            };

            // Act
            var result = await _controller.RegisterUser(invalidUser, _validator);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result.Result!;

            Assert.IsInstanceOfType(objectResult.Value, typeof(ValidationProblemDetails));
            var problemDetails = (ValidationProblemDetails)objectResult.Value!;

            // We check if the controller returned the correct validation error for the FirstName property
            Assert.IsTrue(problemDetails.Errors.ContainsKey("FirstName"));
        }
    }
}
