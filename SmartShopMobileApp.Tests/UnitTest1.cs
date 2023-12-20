using API.Controllers;
using API.Repository;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace SmartShopMobileApp.Tests
{
    public class Tests
    {
        [Test]
        public async Task GetAllSupermarkets_ReturnsOkResult()
        {
            // Arrange
            var unitOfWorkMock = new Mock<IUnitOfWork>(); 
            var supermarketRepositoryMock = new Mock<ISupermarketRepository>(); 

            unitOfWorkMock.Setup(uow => uow.SupermarketRepository.GetAllSupermarkets())
                          .ReturnsAsync(new List<SupermarketDTO>() { });

            var controller = new SupermarketController(unitOfWorkMock.Object);

            // Act
            var result = await controller.GetAllSupermarkets();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsInstanceOf<List<SupermarketDTO>>(okResult.Value);
        }

    }
}