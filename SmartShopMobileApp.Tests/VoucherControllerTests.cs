using API.Controllers;
using API.Models;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SmartShopMobileApp.Tests
{
    [TestFixture]
    public class VoucherControllerTests
    {
        private Mock<IUnitOfWork>? _mockUnitOfWork;
        private Mock<IVoucherRepository>? _mockVoucherRepository;
        private VoucherController? _controller;
        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockVoucherRepository = new Mock<IVoucherRepository>();
            _mockUnitOfWork.Setup(u => u.VoucherRepository).Returns(_mockVoucherRepository.Object);
            _controller = new VoucherController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetVoucherForUserAndSupermarket_ReturnsOk_WhenVoucherExists()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 2;
            var existingVoucher = new VoucherDTO { UserID = userId, SupermarketID = supermarketId };
            _mockVoucherRepository!.Setup(u => u.GetVoucherForUserAndSupermarket(userId, supermarketId))
                .Returns(Task.FromResult(existingVoucher));

            // Act
            var response = await _controller!.GetVoucherForUserAndSupermarket(userId, supermarketId);

            // Assert
            Assert.That(response, Is.InstanceOf<OkObjectResult>());
            var okResult = (OkObjectResult)response;
            Assert.That(okResult.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(okResult.Value, Is.InstanceOf<VoucherDTO>());

        }

        [Test]
        public async Task GetVoucherForUserAndSupermarket_ReturnsBadRequest_ForInvalidUserId()
        {
            // Arrange
            int userId = 0;
            int supermarketId = 2;
            _mockVoucherRepository!.Setup(repo => repo.GetVoucherForUserAndSupermarket(userId, supermarketId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var response = await _controller!.GetVoucherForUserAndSupermarket(userId, supermarketId);

            // Assert
            Assert.That(response, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Exception"));
        }
    }
}
