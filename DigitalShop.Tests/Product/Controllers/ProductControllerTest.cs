using DigitalShop.Application.DTOs.ProductDTO;
using DigitalShop.Application.Services.Interface;
using DigitalShop.Application.Validators;
using DigitalShop.Application.Validators.ProductValidator;
using DigitalShop.Controllers;
using DigitalShop.Infrastructure.Entities.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DigitalShop.Tests.Product.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        private ProductController _productController = null!;
        private ProductCreateValidator _validator = null!;
        private IProductService _service = null!;

        [TestInitialize]
        public void SetUp()
        {
            _validator = new ProductCreateValidator();
            _productController = new ProductController(_service);
        }

        [TestMethod]
        public async Task AddProduct_WhenValidationFails_ShouldReturnValidationProblem()
        {
            // Arrange
            var invalidProduct = new ProductCreateDTO()
            {
                ProductName = "",
                ProductDescription = "Beep boop",
                ProductCategory = Category.Laptop,
                ProductCondition = Condition.BrandNew,
                Price = 1200
            };

            // Act
            var result = await _productController.AddProduct(invalidProduct, _validator);

            // Assert

            // We check if the result is of type "ActionResult"
            Assert.IsInstanceOfType(result.Result, typeof(ObjectResult));
            var objectResult = (ObjectResult)result.Result!;

            // We check if the result is actually "ValidationProblemDetails"
            Assert.IsInstanceOfType(objectResult.Value, typeof(ValidationProblemDetails));
            var problemDetails = (ValidationProblemDetails)objectResult.Value!;

            // (Optionally) We check if the dictionary of errors contains the field that failed
            Assert.IsTrue(problemDetails.Errors.ContainsKey("ProductName"));
        }
    }
}
