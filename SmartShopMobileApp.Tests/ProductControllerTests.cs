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
    public class ProductControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IShoppingCartRepository> _mockShoppingCartRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IOfferRepository> _mockOfferRepository;
        private Mock<ICartItemRepository> _mockCartItemRepository;
        private ShoppingCartController _shoppingCartController;
        private ProductController _productController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockShoppingCartRepository = new Mock<IShoppingCartRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockOfferRepository = new Mock<IOfferRepository>();
            _mockCartItemRepository = new Mock<ICartItemRepository>();

            _mockUnitOfWork.Setup(u => u.ShoppingCartRepository).Returns(_mockShoppingCartRepository.Object);
            _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);
            _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.Setup(u => u.OfferRepository).Returns(_mockOfferRepository.Object);
            _mockUnitOfWork.Setup(u => u.CartItemRepository).Returns(_mockCartItemRepository.Object);

            _shoppingCartController = new ShoppingCartController(_mockUnitOfWork.Object);
            _productController = new ProductController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAllProducts_ProductsExist_ReturnsOkWithProducts()
        {
            // Arrange
            var products = new List<ProductDTO>
            {
                new ProductDTO { ProductID = 1, Name = "Product1", Price = 10 },
                new ProductDTO { ProductID = 2, Name = "Product2", Price = 20 }
            };

            _mockProductRepository.Setup(r => r.GetAllProducts())
                .ReturnsAsync(products);

            // Act
            var result = await _productController.GetAllProducts();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(products));
        }

        [Test]
        public async Task GetAllProducts_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockProductRepository.Setup(r => r.GetAllProducts())
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _productController.GetAllProducts();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task GetProductById_ProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            int productId = 1;
            var product = new ProductDTO { ProductID = productId, Name = "Product1", Price = 10 };

            _mockProductRepository.Setup(r => r.GetProductById(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productController.GetProductById(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task GetProductByBarcode_ProductExists_ReturnsOkWithProduct()
        {
            // Arrange
            string barcode = "123456789";
            var product = new ProductDTO { Barcode = barcode, Name = "Product1", Price = 10 };

            _mockProductRepository.Setup(r => r.GetProductByBarcode(barcode))
                .ReturnsAsync(product);

            // Act
            var result = await _productController.GetProductByBarcode(barcode);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task GetProductByBarcode_ProductDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            string barcode = "123456789";

            _mockProductRepository?.Setup(r => r.GetProductByBarcode(barcode))!
                 .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _productController.GetProductByBarcode(barcode);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task AddProductToShoppingCart_ProductAddedSuccessfully_ReturnsOk()
        {
            // Arrange
            int userId = 1;
            int productId = 1;
            int numberOfProducts = 2;
            int supermarketId = 1;
            string barcode = "123456789";
            var product = new ProductDTO { ProductID = productId, Price = 10, Barcode = barcode };
            var user = new UserDTO { UserID = userId, FirstName = "John Doe" };
            var shoppingCart = new ShoppingCartDTO { ShoppingCartID = 1, UserID = userId, SupermarketID = supermarketId, TotalAmount = 0, IsTransacted = false, CreationDate = DateTime.Now };

            _mockUserRepository.Setup(r => r.GetUserByID(userId)).ReturnsAsync(user);
            _mockProductRepository.Setup(r => r.GetProductByIdAndSupermarket(productId, supermarketId)).ReturnsAsync(product);
            _mockShoppingCartRepository?.Setup(r => r.GetLatestShoppingCartForCurrentUser(userId))!.ReturnsAsync((ShoppingCartDTO)null!);
            _mockShoppingCartRepository!.Setup(r => r.AddShoppingCart(It.IsAny<ShoppingCartDTO>())).Returns(Task.CompletedTask);
            _mockShoppingCartRepository.Setup(r => r.GetLatestShoppingCartForCurrentUser(userId)).ReturnsAsync(shoppingCart);
            _mockOfferRepository.Setup(r => r.GetActiveOfferForProduct(productId, supermarketId, It.IsAny<DateTime>())).ReturnsAsync((OfferDTO)null!);
            _mockCartItemRepository.Setup(r => r.AddCartItem(It.IsAny<CartItemDTO>())).Returns(Task.CompletedTask);
            _mockShoppingCartRepository.Setup(r => r.UpdateShoppingCart(It.IsAny<ShoppingCartDTO>())).Returns(Task.CompletedTask);

            // Act
            var result = await _productController.AddProductToShoppingCart(product, productId, numberOfProducts, supermarketId, userId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(product));
        }

        [Test]
        public async Task AddProductToShoppingCart_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int userId = 1;
            int productId = 1;
            int numberOfProducts = 2;
            int supermarketId = 1;
            var product = new ProductDTO { ProductID = productId, Price = 10 };

            _mockUserRepository.Setup(r => r.GetUserByID(userId)).ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _productController.AddProductToShoppingCart(product, productId, numberOfProducts, supermarketId, userId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

    }
}
