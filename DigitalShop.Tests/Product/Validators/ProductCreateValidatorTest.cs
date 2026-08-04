using DigitalShop.Application.DTOs.ProductDTO;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Application.Validators;
using DigitalShop.Application.Validators.ProductValidator;
using DigitalShop.Controllers;
using DigitalShop.Infrastructure.Entities.Enums;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalShop.Tests.Product.Validators
{
    [TestClass]
    public class ProductCreateValidatorTest
    {
        private ProductCreateValidator _validator = null!;
        [TestInitialize]
        public void SetUp()
        {
            _validator = new ProductCreateValidator();
        }

        [TestMethod]
        public async Task ProductCreateValidator_ShouldValidate_Success()
        {
            // Arrange
            var newProduct = new ProductCreateDTO()
            {
                ProductName = "Laptop",
                ProductDescription = "Beep boop",
                ProductCategory = Category.Laptop,
                ProductCondition = Condition.BrandNew,
                Price = 1200
            };

            // Act
            var result = await _validator.TestValidateAsync(newProduct);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
