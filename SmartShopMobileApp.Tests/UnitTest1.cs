using API.Controllers;
using API.Repository;
using API.Repository.Interfaces;
using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace SmartShopMobileApp.Tests
{
    [TestFixture]
    public class Tests
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mockMapper;
        private SupermarketController controller;

        [SetUp]
        public void Setup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mockMapper = new Mock<IMapper>();
            controller = new SupermarketController(unitOfWorkMock.Object);
        }

        [Test]
        public async Task GetAllSupermarkets_ReturnsOkResult()
        {
            // Arrange
            var supermarketRepositoryMock = new Mock<ISupermarketRepository>();

            unitOfWorkMock.Setup(uow => uow.SupermarketRepository.GetAllSupermarkets())
                          .ReturnsAsync(new List<SupermarketDTO>() { });


            // Act
            var result = await controller.GetAllSupermarkets();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.InstanceOf<List<SupermarketDTO>>());
        }

        [Test]
        public async Task GetAllSupermarkets_ReturnsBadRequestResult()
        {
            // Arrange
            var supermarketRepositoryMock = new Mock<ISupermarketRepository>();

            unitOfWorkMock.Setup(uow => uow.SupermarketRepository.GetAllSupermarkets())
                          .Throws(new Exception("Some error message"));


            // Act
            var result = await controller.GetAllSupermarkets();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value, Is.EqualTo("Some error message"));
        }


        [Test]
        public async Task AddSupermarket_ReturnsOkResult()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>(); 
            var supermarketRepositoryMock = new Mock<ISupermarketRepository>();
            var supermarketToAdd = new SupermarketDTO() { Name="Auchan"};


            var supermarketDto = new SupermarketDTO
            {
                Name="Auchan"
            };

            unitOfWorkMock.Setup(uow => uow.SupermarketRepository.AddSupermarket(It.IsAny<SupermarketDTO>()));

            // Act
            var result = await controller.AddSupermarket(supermarketDto);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)result;
            Assert.That(okResult.Value, Is.EqualTo(supermarketDto));
        }

        [Test]
        public async Task AddSupermarket_ReturnsBadRequestResult()
        {
            // Arrange
            var mockMapper = new Mock<IMapper>();
            var supermarketRepositoryMock = new Mock<ISupermarketRepository>();
            var supermarketToAdd = new SupermarketDTO() { Name = "Auchan" };

            var supermarketDto = new SupermarketDTO
            {
                Name = "Auchan"
            };

            unitOfWorkMock.Setup(uow => uow.SupermarketRepository.AddSupermarket(It.IsAny<SupermarketDTO>()))
                .Throws(new Exception("Some error message"));

            // Act
            var result = await controller.AddSupermarket(supermarketDto);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.That(badRequestResult.Value, Is.EqualTo("Some error message"));
        }


    }
}