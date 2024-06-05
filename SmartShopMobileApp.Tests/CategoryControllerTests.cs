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
    internal class CategoryControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private CategoryController _categoryController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            _mockUnitOfWork.Setup(u => u.CategoryRepository).Returns(_mockCategoryRepository.Object);

            _categoryController = new CategoryController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAllCategories_CategoriesExist_ReturnsOkWithCategories()
        {
            // Arrange
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO { CategoryID = 1, Name = "Category 1" },
                new CategoryDTO { CategoryID = 2, Name = "Category 2" }
            };

            _mockCategoryRepository.Setup(r => r.GetAllCategories())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryController.GetAllCategories();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var resultCategories = okResult.Value as IEnumerable<CategoryDTO>;
            Assert.That(resultCategories, Is.Not.Null);
            Assert.That(resultCategories.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllCategories_NoCategoriesExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            var categories = new List<CategoryDTO>();

            _mockCategoryRepository.Setup(r => r.GetAllCategories())
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryController.GetAllCategories();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);

            var resultCategories = okResult.Value as IEnumerable<CategoryDTO>;
            Assert.That(resultCategories, Is.Not.Null);
            Assert.That(resultCategories, Is.Empty);
        }

        [Test]
        public async Task GetAllCategories_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockCategoryRepository.Setup(r => r.GetAllCategories())
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _categoryController.GetAllCategories();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }
    }
}
