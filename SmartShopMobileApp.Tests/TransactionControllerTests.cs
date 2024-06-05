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
    public class TransactionControllerTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private TransactionController _transactionController;

        [SetUp]
        public void SetUp()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();

            _mockUnitOfWork.Setup(u => u.TransactionRepository).Returns(_mockTransactionRepository.Object);
            _transactionController = new TransactionController(_mockUnitOfWork.Object);
        }

        [Test]
        public async Task GetAllTransactions_TransactionsExist_ReturnsOkWithTransactions()
        {
            // Arrange
            var transactions = new List<TransactionDTO>
            {
                new TransactionDTO { TransactionID = 1, TotalAmount = 50 },
                new TransactionDTO { TransactionID = 2, TotalAmount = 100 }
            };

            _mockTransactionRepository.Setup(r => r.GetAllTransactions())
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionController.GetAllTransactions();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(transactions));
        }

        [Test]
        public async Task GetAllTransactions_NoTransactionsExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            var transactions = new List<TransactionDTO>();

            _mockTransactionRepository.Setup(r => r.GetAllTransactions())
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionController.GetAllTransactions();

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(transactions));
        }

        [Test]
        public async Task GetAllTransactions_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            _mockTransactionRepository.Setup(r => r.GetAllTransactions())
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _transactionController.GetAllTransactions();

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult!.Value, Is.EqualTo("Exception"));
        }

        [Test]
        public async Task GetAllTransactionsWithDiscount_TransactionsExist_ReturnsOkWithTransactions()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            var transactions = new List<TransactionDTO>
            {
                new TransactionDTO { TransactionID = 1, TotalAmount = 50 },
                new TransactionDTO { TransactionID = 2, TotalAmount = 100 }
            };

            _mockTransactionRepository.Setup(r => r.GetAllTransactionsWithDiscount(userId, supermarketId))
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionController.GetAllTransactionsWithDiscount(userId, supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(transactions));
        }

        [Test]
        public async Task GetAllTransactionsWithDiscount_NoTransactionsExist_ReturnsOkWithEmptyList()
        {
            // Arrange
            int userId = 1;
            int supermarketId = 1;
            var transactions = new List<TransactionDTO>();

            _mockTransactionRepository.Setup(r => r.GetAllTransactionsWithDiscount(userId, supermarketId))
                .ReturnsAsync(transactions);

            // Act
            var result = await _transactionController.GetAllTransactionsWithDiscount(userId, supermarketId);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(transactions));
        }

        [Test]
        public async Task AddTransaction_ValidTransaction_ReturnsOkWithTransaction()
        {
            // Arrange
            var transaction = new TransactionDTO { TransactionID = 1, TotalAmount = 50 };

            _mockTransactionRepository.Setup(r => r.AddTransaction(transaction))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _transactionController.AddTransaction(transaction);

            // Assert
            Assert.That(result, Is.InstanceOf<OkObjectResult>());
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.Value, Is.EqualTo(transaction));
        }

        [Test]
        public async Task AddTransaction_ExceptionThrown_ReturnsBadRequest()
        {
            // Arrange
            var transaction = new TransactionDTO { TransactionID = 1, TotalAmount = 50 };

            _mockTransactionRepository.Setup(r => r.AddTransaction(transaction))
                .ThrowsAsync(new Exception("Exception"));

            // Act
            var result = await _transactionController.AddTransaction(transaction);

            // Assert
            Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Exception"));
        }
    }
}
