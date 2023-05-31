using AutoMapper;
using MediatR;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Customers.Dtos;

namespace TransactionApp.Application.Customers.Queries
{
    public class GetCustomersByIdQuery : IRequest<CustomersDto>
    {
        public Guid Id { get; set; }
    }

    public class GetCustomersByIdQueryHandler : IRequestHandler<GetCustomersByIdQuery, CustomersDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetCustomersByIdQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<CustomersDto> Handle(GetCustomersByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new Common.Exceptions.ApplicationException("Invalid Id");
            }
            var customer = await _repository.GetByIdAsync<Domain.Entities.Customers>(request.Id);

            return customer != null ? _mapper.Map<CustomersDto>(customer) : throw new NotFoundException(nameof(Domain.Entities.Customers), request.Id);
        }
    }
}
