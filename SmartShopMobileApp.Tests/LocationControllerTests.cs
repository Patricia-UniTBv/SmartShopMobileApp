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
    public class LocationControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ILocationRepository> _mockLocationRepository;
        private LocationController _locationController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLocationRepository = new Mock<ILocationRepository>();

            _mockUnitOfWork.Setup(u => u.LocationRepository).Returns(_mockLocationRepository.Object);

            _locationController = new LocationController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetLocationsBySupermarketId_LocationsExist_ReturnsOkWithLocations()
        {
            // Arrange
            int supermarketId = 1;
            var locations = new List<LocationDTO>
            {
                new LocationDTO { LocationID = 1, Address = "Location 1" },
                new LocationDTO { LocationID = 2, Address = "Location 2" }
            };
    
            _mockLocationRepository.Setup(r => r.GetLocationsForSupermarketId(supermarketId))
                .ReturnsAsync(locations);

            // Act
            var result = await _locationController.GetLocationsBySupermarketId(supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var resultLocations = okResult.Value as IEnumerable<LocationDTO>;
            Assert.That(resultLocations, Is.Not.Null);
            Assert.That(resultLocations.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetLocationsBySupermarketId_NoLocationsExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            int supermarketId = 1;
            var locations = new List<LocationDTO>();

            _mockLocationRepository.Setup(r => r.GetLocationsForSupermarketId(supermarketId))
                .ReturnsAsync(locations);

            // Act
            var result = await _locationController.GetLocationsBySupermarketId(supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            var resultLocations = okResult.Value as IEnumerable<LocationDTO>;
            Assert.That(resultLocations, Is.Not.Null);
            Assert.That(resultLocations, Is.Empty);
        }

        [Test]
        public async Task GetLocationsBySupermarketId_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int supermarketId = 1;

            _mockLocationRepository.Setup(r => r.GetLocationsForSupermarketId(supermarketId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _locationController.GetLocationsBySupermarketId(supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }
    }
}
