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

        [Test]
        public async Task UpdateVoucher_VoucherNotFound_ReturnsNotFound()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            decimal totalAmount = 100m;
            _mockVoucherRepository?.Setup(r => r.GetVoucherForUserAndSupermarket(userId, supermarketId))!
                .ReturnsAsync((VoucherDTO)null!);

            // Act
            var result = await _controller!.UpdateVoucher(userId, supermarketId, totalAmount);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        }

        [Test]
        public async Task UpdateVoucher_VoucherFound_UpdatesVoucherAndReturnsOk()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            decimal totalAmount = 200m;
            var existingVoucher = new VoucherDTO { UserID = userId, SupermarketID = supermarketId, EarnedPoints = 0 };
            _mockVoucherRepository!.Setup(r => r.GetVoucherForUserAndSupermarket(userId, supermarketId))
                .ReturnsAsync(existingVoucher);

            // Act
            var result = await _controller!.UpdateVoucher(userId, supermarketId, totalAmount);

            // Assert
            _mockVoucherRepository.Verify(r => r.UpdateVoucherForSpecificUser(It.Is<VoucherDTO>(v => v.EarnedPoints == 0.1m)), Times.Once);
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateVoucher_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            decimal totalAmount = 200m;
            _mockVoucherRepository!.Setup(r => r.GetVoucherForUserAndSupermarket(userId, supermarketId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _controller!.UpdateVoucher(userId, supermarketId, totalAmount);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task CreateVoucherForUser_ValidInput_CreatesVoucherAndReturnsOk()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            VoucherDTO newVoucher = null;

            _mockVoucherRepository!.Setup(r => r.CreateVoucherForUserAndSupermarket(It.IsAny<VoucherDTO>()))
                .Callback<VoucherDTO>(voucher => newVoucher = voucher)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller!.CreateVoucherForUser(userId, supermarketId);

            // Assert
            Assert.That(newVoucher, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(newVoucher.UserID, Is.EqualTo(userId));
                Assert.That(newVoucher.SupermarketID, Is.EqualTo(supermarketId));
                Assert.That(newVoucher.EarnedPoints, Is.EqualTo(0));
            });
        }

        [Test]
        public async Task CreateVoucherForUser_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;

            _mockVoucherRepository!.Setup(r => r.CreateVoucherForUserAndSupermarket(It.IsAny<VoucherDTO>()))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _controller!.CreateVoucherForUser(userId, supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }
    }
}
