using API.Controllers;
using API.Repository.Interfaces;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace SmartShopMobileApp.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUnitOfWork>? _mockUnitOfWork;
        private Mock<IUserRepository>? _mockUserRepository;
        private UserController? _controller;

        [SetUp]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUserRepository = new Mock<IUserRepository>();

            _mockUnitOfWork.Setup(u => u.UserRepository).Returns(_mockUserRepository.Object);

            _controller = new UserController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAllUsers_ReturnsOkResult_WithListOfUsers()
        {
            // Arrange
            var userList = new List<UserDTO>
            {
                new UserDTO { UserID = 1, FirstName = "Patricia" },
                new UserDTO { UserID = 2, FirstName = "Andrei" }
            };

            _mockUserRepository?.Setup(repo => repo.GetAllUsers())
                .ReturnsAsync(userList);

            // Act
            var result = await _controller!.GetAllUsers();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsInstanceOf<List<UserDTO>>(okResult.Value);
            var users = okResult.Value as List<UserDTO>;
            Assert.That(users?.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllUsers_ReturnsBadRequest_OnException()
        {
            // Arrange
            _mockUserRepository!.Setup(repo => repo.GetAllUsers())
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _controller!.GetAllUsers();

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task GetUserByEmailAndPassword_ReturnsOkResult_WithUser()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";
            var userDto = new UserDTO { UserID = 1, Email = email, FirstName = "Patricia" };

            _mockUserRepository!.Setup(repo => repo.GetUserByEmailAndPassword(email, password))
                .ReturnsAsync(userDto);

            // Act
            var result = await _controller!.GetUserByEmailAndPassword(email, password);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.IsInstanceOf<UserDTO>(okResult.Value);
            var user = okResult.Value as UserDTO;
            Assert.That(user!.Email, Is.EqualTo(email));
            Assert.That(user.FirstName, Is.EqualTo("Patricia"));
        }

        [Test]
        public async Task GetUserByEmailAndPassword_ReturnsBadRequest_OnException()
        {
            // Arrange
            var email = "test@example.com";
            var password = "password";

            _mockUserRepository!.Setup(repo => repo.GetUserByEmailAndPassword(email, password))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _controller!.GetUserByEmailAndPassword(email, password);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task GetPreferredLanguageAndCurrency_ReturnsOkResult_WithLanguageAndCurrency()
        {
            // Arrange
            var userId = 1;
            var language = "en";
            var currency = "RON";
            var expectedResult = Tuple.Create(language, currency);

            _mockUserRepository!.Setup(repo => repo.GetPreferredLanguageAndCurrency(userId))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller!.GetPreferredLanguageAndCurrency(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.That(okResult.Value, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task GetPreferredLanguageAndCurrency_ReturnsBadRequest_OnException()
        {
            // Arrange
            var userId = 1;

            _mockUserRepository!.Setup(repo => repo.GetPreferredLanguageAndCurrency(userId))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _controller!.GetPreferredLanguageAndCurrency(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.That(badRequestResult.Value, Is.EqualTo("Exception"));
        }
    }
}