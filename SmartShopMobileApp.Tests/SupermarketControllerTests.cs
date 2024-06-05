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
    public class SupermarketControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ISupermarketRepository> _mockSupermarketRepository;
        private SupermarketController _supermarketController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockSupermarketRepository = new Mock<ISupermarketRepository>();

            _mockUnitOfWork.Setup(u => u.SupermarketRepository).Returns(_mockSupermarketRepository.Object);
            _supermarketController = new SupermarketController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAllSupermarkets_SupermarketsExist_ReturnsOkWithSupermarkets()
        {
            // Arrange
            var supermarkets = new List<SupermarketDTO>
            {
                new() { SupermarketID = 1, Name = "Supermarket 1" },
                new() { SupermarketID = 2, Name = "Supermarket 2" }
            };

            _mockSupermarketRepository.Setup(r => r.GetAllSupermarkets())
                .ReturnsAsync(supermarkets);

            // Act
            var result = await _supermarketController.GetAllSupermarkets();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(supermarkets));
        }

        [Test]
        public async Task GetAllSupermarkets_NoSupermarketsExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            var supermarkets = new List<SupermarketDTO>();

            _mockSupermarketRepository.Setup(r => r.GetAllSupermarkets())
                .ReturnsAsync(supermarkets);

            // Act
            var result = await _supermarketController.GetAllSupermarkets();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(supermarkets));
        }

        [Test]
        public async Task GetAllSupermarkets_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockSupermarketRepository.Setup(r => r.GetAllSupermarkets())
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _supermarketController.GetAllSupermarkets();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task GetSupermarketById_SupermarketExists_ReturnsOkWithSupermarket()
        {
            // Arrange
            int supermarketId = 1;
            var supermarket = new SupermarketDTO { SupermarketID = supermarketId, Name = "Test Supermarket" };

            _mockSupermarketRepository.Setup(r => r.GetSupermarketById(supermarketId))
                .ReturnsAsync(supermarket);

            // Act
            var result = await _supermarketController.GetSupermarketById(supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(supermarket));
        }

        [Test]
        public async Task GetSupermarketById_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int supermarketId = 1;

            _mockSupermarketRepository.Setup(r => r.GetSupermarketById(supermarketId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _supermarketController.GetSupermarketById(supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }
    }
}
