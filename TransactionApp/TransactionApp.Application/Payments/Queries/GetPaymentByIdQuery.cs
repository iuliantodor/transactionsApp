using AutoMapper;
using MediatR;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Payments.Dtos;

namespace TransactionApp.Application.Payments.Queries
{
    public class GetPaymentByIdQuery : IRequest<PaymentsDto>
    {
        public Guid Id { get; set; }
    }

    public class GetPaymentByIdQueryHandler : IRequestHandler<GetPaymentByIdQuery, PaymentsDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetPaymentByIdQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<PaymentsDto> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new Common.Exceptions.ApplicationException("Invalid Id");
            }
            var payment = await _repository.GetByIdAsync<Domain.Entities.Payments>(request.Id);

            return payment != null ? _mapper.Map<PaymentsDto>(payment) : throw new NotFoundException(nameof(Domain.Entities.Payments), request.Id);
        }
    }
}
