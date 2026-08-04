using DigitalShop.Application.DTOs.CartDTO;
using DigitalShop.Application.Validators.CartValidators;
using FluentValidation.TestHelper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Tests.Cart.Validators
{
    [TestClass]
    public class CartCreateValidatorTest
    {
        private CartCreateValidator _validator = null!;
        [TestInitialize]
        public void SetUp()
        {
            _validator = new CartCreateValidator();
        }

        [TestMethod]
        public async Task CartCreateValidator_ShouldValidate_Success()
        {
            // Arrange
            var newCart = new CartCreateDTO()
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1
            };

            // Act
            var result = await _validator.TestValidateAsync(newCart);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
