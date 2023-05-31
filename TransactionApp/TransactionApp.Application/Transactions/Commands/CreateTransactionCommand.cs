using AutoMapper;
using MediatR;
using TransactionApp.Application.Articles.Dtos;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Payments.Dtos;
using TransactionApp.Application.Transactions.Dtos;
using TransactionApp.Domain.Entities;

namespace TransactionApp.Application.Transactions.Commands
{
    public class CreateTransactionCommand : IRequest<TransactionsDto>
    {
        public DateTime TransactionDate { get; set; }

        public List<Guid> PaymentsId { get; set; }

        public List<Guid> ArticlesId { get; set; }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionsDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public CreateTransactionCommandHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TransactionsDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = new Domain.Entities.Transactions
            {
                TransactionDate = DateTime.Now,
                TransactionArticles = new List<TransactionArticle>(),
            };

            //articles list
            foreach (var articleItem in request.ArticlesId)
            {
                var article = GetArticleBy(articleItem);

                //check if article has a payments
                var articlePayment = _repository.Get<Domain.Entities.Payments>().Where(p => p.ArticleId == article.Id);

                if (articlePayment == null)
                {
                    throw new BadRequestException($"Payment not found for: {article.Name}.");
                }

                foreach (var paymentItem in articlePayment)
                {
                    ProceedTransaction(paymentItem, article);
                }

                //create the transactionArticle
                var transactionArticle = new TransactionArticle
                {
                    Articles = article,
                    Transaction = transaction
                };
                transaction.TransactionArticles.Add(transactionArticle);
            }

            //payments list
            foreach (var paymentItem in request.PaymentsId)
            {
                var payment = await _repository.GetByIdAsync<Domain.Entities.Payments>(paymentItem);
                if (payment == null)
                {
                    throw new NotFoundException(nameof(PaymentsDto), paymentItem);
                }

                var article = GetArticleBy(payment.ArticleId);
                ProceedTransaction(payment, article);

                var transactionArticle = new TransactionArticle
                {
                    Articles = article,
                    Transaction = transaction
                };
                transaction.TransactionArticles.Add(transactionArticle);
            }

            await _repository.InsertAsync(transaction);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<TransactionsDto>(transaction);
        }

        private Domain.Entities.Articles GetArticleBy(Guid Id)
        {
            var article = _repository.GetById<Domain.Entities.Articles>(Id);

            if (article == null)
            {
                throw new NotFoundException(nameof(ArticlesDto), Id);
            }
            return article;
        }

        private void ProceedTransaction(
            Domain.Entities.Payments payments, 
            Domain.Entities.Articles article)
        {
            if (payments.Status != PaymentStatus.Completed)
            {
                if (article.StockQuantity < payments.QuantitySold)
                {
                    throw new BadRequestException($"Insufficient stock quantity for article {article.Name}.");
                }
                article.StockQuantity -= payments.QuantitySold;
                payments.Status = PaymentStatus.Completed;

                _repository.Update(payments);
            }
            _repository.Update(article);
        }
    }
}
