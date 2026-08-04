using DigitalShop.Application.DTOs.CartDTO;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Application.Validators.CartValidators;
using DigitalShop.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace DigitalShop.Tests.Cart.Controllers
{
    [TestClass]
    public class CartControllerTest
    {
        private CartController _cartController = null!;
        private CartCreateValidator _validator = null!;
        private ICartService _service = null!;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new CartCreateValidator();
            _cartController = new CartController(_service);

            // We need to add a user mock to the controller's HttpContext for testing
            var fakeUserId = Guid.NewGuid().ToString();
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, fakeUserId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            _cartController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = claimsPrincipal }
            };
        }

        [TestMethod]
        public async Task AddCart_WhenValidationFails_ShouldReturnValidationProblem()
        {
            // Arrange
            var invalidCart = new CartCreateDTO()
            {
                ProductId = Guid.Empty, // Invalid product id (not a guid)
                Quantity = -1 // Invalid quantity (negative number)
            };

            // Act
            var result = await _cartController.AddCart(invalidCart, _validator);

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result.Result;

            Assert.IsInstanceOfType(objectResult.Value, typeof(ValidationProblemDetails));
            var problemDetails = (ValidationProblemDetails)objectResult.Value!;

            Assert.IsTrue(problemDetails.Errors.ContainsKey("ProductId"));
        }
    }
}
