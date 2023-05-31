using AutoMapper;
using Moq;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Transactions.Commands;
using TransactionApp.Application.Transactions.Dtos;
using TransactionApp.Domain.Entities;

namespace TransactionApp.NUnitTests
{
    [TestFixture]
    public class TransactionsControllerTest
    {
        private Mock<IRepository> _repositoryMock;
        private Mock<IMapper> _mapperMock;
        private CreateTransactionCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateTransactionCommandHandler(_repositoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public void Handle_PaymentNotFound_ThrowsBadRequestException()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                TransactionDate = DateTime.Now,
                PaymentsId = new List<Guid> { Guid.NewGuid() },
                ArticlesId = new List<Guid> { Guid.NewGuid() }
            };

            _repositoryMock.Setup(r => r.GetByIdAsync<Payments>(command.PaymentsId[0])).ReturnsAsync((Payments)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        }

        [Test]
        public void Handle_ArticleNotFound_ThrowsNotFoundException()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                TransactionDate = DateTime.Now,
                PaymentsId = new List<Guid> { Guid.NewGuid() },
                ArticlesId = new List<Guid> { Guid.NewGuid() }
            };

            var payment = new Payments { Id = command.PaymentsId[0], ArticleId = Guid.NewGuid() };

            _repositoryMock.Setup(r => r.GetByIdAsync<Payments>(command.PaymentsId[0])).ReturnsAsync(payment);
            _repositoryMock.Setup(r => r.GetById<Articles>(payment.ArticleId)).Returns((Articles)null);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, default));
        }

        [Test]
        public void Handle_InsufficientStock_ThrowsBadRequestException()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                TransactionDate = DateTime.Now,
                PaymentsId = new List<Guid> { Guid.NewGuid() },
                ArticlesId = new List<Guid> { Guid.NewGuid() }
            };

            var payment = new Payments { Id = command.PaymentsId[0], ArticleId = command.ArticlesId[0], QuantitySold = 10 };
            var article = new Articles { Id = command.ArticlesId[0], StockQuantity = 5 };

            _repositoryMock.Setup(r => r.GetByIdAsync<Payments>(command.PaymentsId[0])).ReturnsAsync(payment);
            _repositoryMock.Setup(r => r.GetById<Articles>(command.ArticlesId[0])).Returns(article);

            // Act & Assert
            Assert.ThrowsAsync<BadRequestException>(() => _handler.Handle(command, default));
        }

        [Test]
        public async Task Handle_IncompletePayment_CompletesPayment()
        {
            // Arrange
            var command = new CreateTransactionCommand
            {
                TransactionDate = DateTime.Now,
                PaymentsId = new List<Guid> { Guid.NewGuid() },
                ArticlesId = new List<Guid> { Guid.NewGuid() }
            };

            var payment = new Payments { Id = command.PaymentsId[0], ArticleId = command.ArticlesId[0], Status = PaymentStatus.Pending };
            var article = new Articles { Id = command.ArticlesId[0], StockQuantity = 10 };

            _repositoryMock.Setup(r => r.GetByIdAsync<Payments>(command.PaymentsId[0])).ReturnsAsync(payment);
            _repositoryMock.Setup(r => r.GetById<Articles>(command.ArticlesId[0])).Returns(article);
            _repositoryMock.Setup(r => r.Update(payment)).Verifiable();
            _repositoryMock.Setup(r => r.Update(article)).Verifiable();

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.AreEqual(PaymentStatus.Completed, payment.Status);
            _repositoryMock.Verify(r => r.Update(payment), Times.Once);
            _repositoryMock.Verify(r => r.Update(article), Times.Once);
        }
    }
}
