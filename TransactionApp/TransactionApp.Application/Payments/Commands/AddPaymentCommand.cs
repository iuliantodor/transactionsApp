using AutoMapper;
using MediatR;
using TransactionApp.Application.Articles.Dtos;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Payments.Dtos;

namespace TransactionApp.Application.Payments.Commands
{
    public class AddPaymentCommand : IRequest<PaymentsDto>
    {
        public Guid CustomerId { get; set; }

        public DateTime PaymentDate { get; set; }

        public int QuantitySold { get; set; }

        public Guid ArticleId { get; set; }
    }

    public class AddPaymentCommandHandler : IRequestHandler<AddPaymentCommand, PaymentsDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public AddPaymentCommandHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentsDto> Handle(AddPaymentCommand request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetByIdAsync<Domain.Entities.Articles>(request.ArticleId);
            if (article == null)
            {
                throw new NotFoundException(nameof(ArticlesDto), request.ArticleId);
            }

            var payment = new Domain.Entities.Payments
            {
                CustomerId = request.CustomerId,
                PaymentDate = request.PaymentDate,
                ArticleId = request.ArticleId,
                QuantitySold = request.QuantitySold,
                Amount = article.Price * request.QuantitySold,
                Status = PaymentStatus.Pending,
            };

            await _repository.InsertAsync(payment);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PaymentsDto>(payment);
        }
    }
}
