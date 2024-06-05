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
    public class OfferControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<IOfferRepository> _mockOfferRepository;
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<ISupermarketRepository> _mockSupermarketRepository;
        private OfferController _offerController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockOfferRepository = new Mock<IOfferRepository>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockSupermarketRepository = new Mock<ISupermarketRepository>();

            _mockUnitOfWork.Setup(u => u.OfferRepository).Returns(_mockOfferRepository.Object);
            _mockUnitOfWork.Setup(u => u.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.Setup(u => u.SupermarketRepository).Returns(_mockSupermarketRepository.Object);

            _offerController = new OfferController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAllCurrentOffers_OffersExist_ReturnsOkWithOffers()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var offers = new List<OfferDTO>
            {
                new OfferDTO { OfferId = 1, ProductId = 1, SupermarketId = 1, OfferStartDate = currentDate.AddDays(-1), OfferEndDate = currentDate.AddDays(1) },
                new OfferDTO { OfferId = 2, ProductId = 2, SupermarketId = 2, OfferStartDate = currentDate.AddDays(-1), OfferEndDate = currentDate.AddDays(1) }
            };

            _mockOfferRepository.Setup(r => r.GetAllCurrentOffers())
                .ReturnsAsync(offers);

            _mockProductRepository.Setup(r => r.GetProductById(It.IsAny<int>()))
                .ReturnsAsync((int id) => new ProductDTO { ProductID = id });

            _mockSupermarketRepository.Setup(r => r.GetSupermarketById(It.IsAny<int>()))
                .ReturnsAsync((int id) => new SupermarketDTO { SupermarketID = id });

            // Act
            var result = await _offerController.GetAllCurrentOffers();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var resultOffers = okResult.Value as IEnumerable<OfferDTO>;
            Assert.That(resultOffers, Is.Not.Null);
            Assert.That(resultOffers.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllCurrentOffers_NoOffersExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var offers = new List<OfferDTO>();

            _mockOfferRepository.Setup(r => r.GetAllCurrentOffers())
                .ReturnsAsync(offers);

            // Act
            var result = await _offerController.GetAllCurrentOffers();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var resultOffers = okResult.Value as IEnumerable<OfferDTO>;
            Assert.That(resultOffers, Is.Not.Null);
            Assert.That(resultOffers, Is.Empty);
        }

        [Test]
        public async Task GetAllCurrentOffers_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockOfferRepository.Setup(r => r.GetAllCurrentOffers())
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _offerController.GetAllCurrentOffers();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }
    }
}
