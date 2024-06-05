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
    public class CartItemControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICartItemRepository> _mockCartItemRepository;
        private Mock<IShoppingCartRepository> _mockShoppingCartRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IOfferRepository> _mockOfferRepository;
        private ShoppingCartController _shoppingCartController;
        private CartItemController _cartItemController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartItemRepository = new Mock<ICartItemRepository>();
            _mockShoppingCartRepository = new Mock<IShoppingCartRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockOfferRepository = new Mock<IOfferRepository>();

            _mockUnitOfWork.Setup(u => u.CartItemRepository).Returns(_mockCartItemRepository.Object);
            _mockUnitOfWork.Setup(u => u.ShoppingCartRepository).Returns(_mockShoppingCartRepository.Object);
            _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.Setup(u => u.OfferRepository).Returns(_mockOfferRepository.Object);

            _shoppingCartController = new ShoppingCartController(_mockUnitOfWork.Object);
            _cartItemController = new CartItemController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetItemsForShoppingCart_ItemsExist_ReturnsOkWithItems()
        {
            // Arrange
            int shoppingCartId = 1;
            var cartItems = new List<ProductDTO>
            {
                new ProductDTO { ProductID = 1, Name = "Product 1", Price = 10, Quantity = 2 },
                new ProductDTO { ProductID = 2, Name = "Product 2", Price = 20, Quantity = 3 }
            };

            _mockCartItemRepository.Setup(r => r.GetProductsByShoppingCart(shoppingCartId))
                .ReturnsAsync(cartItems);

            // Act
            var result = await _cartItemController.GetItemsForShoppingCart(shoppingCartId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var resultItems = okResult.Value as IEnumerable<ProductDTO>;
            Assert.That(resultItems, Is.Not.Null);
            Assert.That(resultItems.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetItemsForShoppingCart_NoItemsExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            int shoppingCartId = 1;
            var cartItems = new List<ProductDTO>();

            _mockCartItemRepository.Setup(r => r.GetProductsByShoppingCart(shoppingCartId))
                .ReturnsAsync(cartItems);

            // Act
            var result = await _cartItemController.GetItemsForShoppingCart(shoppingCartId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var resultItems = okResult.Value as IEnumerable<ProductDTO>;
            Assert.IsNotNull(resultItems);
            Assert.IsEmpty(resultItems);
        }

        [Test]
        public async Task GetItemsForShoppingCart_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int shoppingCartId = 1;

            _mockCartItemRepository.Setup(r => r.GetProductsByShoppingCart(shoppingCartId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _cartItemController.GetItemsForShoppingCart(shoppingCartId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task DeleteCartItemFromShoppingCart_CartItemExists_ReturnsOk()
        {
            // Arrange
            int productId = 1;
            int shoppingCartId = 1;
            double quantity = 2;
            int userId = 1;

            var cartItem = new CartItemDTO { ProductID = productId, ShoppingCartID = shoppingCartId, Quantity = quantity };
            var shoppingCart = new ShoppingCartDTO { UserID = userId, SupermarketID = 1, TotalAmount = 100 };
            var product = new ProductDTO { ProductID = productId, Price = 10 };

            _mockCartItemRepository.Setup(r => r.GetCartItemByProductIdAndShoppingCartId(productId, shoppingCartId, quantity))
                .ReturnsAsync(cartItem);

            _mockShoppingCartRepository.Setup(r => r.GetLatestShoppingCartForCurrentUser(userId))
                .ReturnsAsync(shoppingCart);

            _mockProductRepository.Setup(r => r.GetProductById(productId))
                .ReturnsAsync(product);

            _mockOfferRepository.Setup(r => r.GetActiveOfferForProduct(productId, shoppingCart.SupermarketID.Value, It.IsAny<DateTime>()))
                .ReturnsAsync((int id, int supermarketId, DateTime currentDate) => null!);

            // Act
            var result = await _cartItemController.DeleteCartItemFromShoppingCart(productId, shoppingCartId, quantity, userId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var deletedCartItem = okResult.Value as CartItemDTO;
            Assert.That(deletedCartItem, Is.Not.Null);
            Assert.That(deletedCartItem.ProductID, Is.EqualTo(productId));
        }

        [Test]
        public async Task DeleteCartItemFromShoppingCart_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int productId = 1;
            int shoppingCartId = 1;
            double quantity = 2;
            int userId = 1;

            _mockCartItemRepository.Setup(r => r.GetCartItemByProductIdAndShoppingCartId(productId, shoppingCartId, quantity))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _cartItemController.DeleteCartItemFromShoppingCart(productId, shoppingCartId, quantity, userId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

    }
}
