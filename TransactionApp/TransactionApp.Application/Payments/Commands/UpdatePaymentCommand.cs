using AutoMapper;
using MediatR;
using TransactionApp.Application.Articles.Dtos;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Payments.Dtos;
using TransactionApp.Domain.Entities;

namespace TransactionApp.Application.Payments.Commands
{
    public class UpdatePaymentCommand : IRequest<PaymentsDto>
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public DateTime PaymentDate { get; set; }

        public int QuantitySold { get; set; }

        public Guid ArticleId { get; set; }
    }

    public class UpdatePaymentCommandHandler : IRequestHandler<UpdatePaymentCommand, PaymentsDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePaymentCommandHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaymentsDto> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _repository.GetByIdAsync<Domain.Entities.Payments>(request.Id);
            if (payment == null)
            {
                throw new NotFoundException(nameof(PaymentsDto), request.Id);
            }

            var article = await _repository.GetByIdAsync<Domain.Entities.Articles>(request.ArticleId);
            if (article == null)
            {
                throw new NotFoundException(nameof(ArticlesDto), request.ArticleId);
            }

            payment.CustomerId = request.CustomerId;
            payment.PaymentDate = request.PaymentDate;
            payment.ArticleId = request.ArticleId;
            payment.QuantitySold = request.QuantitySold;
            payment.Amount = article.Price * request.QuantitySold;
            payment.Status = PaymentStatus.Pending;

            _repository.Update(payment);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<PaymentsDto>(payment);
        }
    }
}
