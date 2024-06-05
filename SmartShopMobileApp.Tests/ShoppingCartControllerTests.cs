using API.Controllers;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Tests
{
    [TestFixture]
    public class ShoppingCartControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IShoppingCartRepository> _mockShoppingCartRepository;
        private ShoppingCartController _shoppingCartController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockShoppingCartRepository = new Mock<IShoppingCartRepository>();

            _mockUnitOfWork.Setup(u => u.ShoppingCartRepository).Returns(_mockShoppingCartRepository.Object);
            _shoppingCartController = new ShoppingCartController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAllTransactedShoppingCartsByUserId_ShoppingCartsExist_ReturnsOkWithShoppingCarts()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            var shoppingCarts = new List<ShoppingCartDTO>
            {
                new ShoppingCartDTO { ShoppingCartID = 1, UserID = userId, SupermarketID = supermarketId, IsTransacted = true, TotalAmount = 100 },
                new ShoppingCartDTO { ShoppingCartID = 2, UserID = userId, SupermarketID = supermarketId, IsTransacted = true, TotalAmount = 150 }
            };

            _mockShoppingCartRepository.Setup(r => r.GetAllTransactedShoppingCartsByUserId(userId, supermarketId))
                .ReturnsAsync(shoppingCarts);

            // Act
            var result = await _shoppingCartController.GetAllTransactedShoppingCartsByUserId(userId, supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(shoppingCarts));
        }

        [Test]
        public async Task GetAllTransactedShoppingCartsByUserId_NoShoppingCartsExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            var shoppingCarts = new List<ShoppingCartDTO>();

            _mockShoppingCartRepository.Setup(r => r.GetAllTransactedShoppingCartsByUserId(userId, supermarketId))
                .ReturnsAsync(shoppingCarts);

            // Act
            var result = await _shoppingCartController.GetAllTransactedShoppingCartsByUserId(userId, supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(shoppingCarts));
        }

        [Test]
        public async Task GetAllTransactedShoppingCartsByUserId_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;

            _mockShoppingCartRepository.Setup(r => r.GetAllTransactedShoppingCartsByUserId(userId, supermarketId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _shoppingCartController.GetAllTransactedShoppingCartsByUserId(userId, supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task UpdateShoppingCartWhenTransacted_ShoppingCartExists_ReturnsOkWithUpdatedShoppingCart()
        {
            // Arrange
            int shoppingCartId = 1;
            var shoppingCart = new ShoppingCartDTO { ShoppingCartID = shoppingCartId, IsTransacted = false };

            _mockShoppingCartRepository.Setup(r => r.GetShoppingCartById(shoppingCartId))
                .ReturnsAsync(shoppingCart);
            _mockShoppingCartRepository.Setup(r => r.UpdateShoppingCart(shoppingCart))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _shoppingCartController.UpdateShoppingCartWhenTransacted(shoppingCartId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(shoppingCart));
            Assert.That(shoppingCart.IsTransacted, Is.True);
        }

        [Test]
        public async Task UpdateShoppingCartWhenTransacted_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int shoppingCartId = 1;

            _mockShoppingCartRepository.Setup(r => r.GetShoppingCartById(shoppingCartId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _shoppingCartController.UpdateShoppingCartWhenTransacted(shoppingCartId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }
    }
}
