using AutoMapper;
using MediatR;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Customers.Dtos;
using WorkplaceBooking.Application.Common.Models;

namespace TransactionApp.Application.Customers.Queries
{
    public class GetCustomerWithPaginationQuery : IRequest<PaginatedList<CustomersDto>>
    {
        public int PageNumber { get; init; } = PagedConstants.PageNumber;
        public int PageSize { get; init; } = PagedConstants.PageSize;
    }

    public class GetCustomerWithPaginationQueryHandler : IRequestHandler<GetCustomerWithPaginationQuery, PaginatedList<CustomersDto>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetCustomerWithPaginationQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CustomersDto>> Handle(GetCustomerWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PaginatedList<CustomersDto>>(
                                await _repository.GetPaginated<Domain.Entities.Customers>(
                                                    request.PageNumber,
                                                    request.PageSize));
        }
    }
}
