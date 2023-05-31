using AutoMapper;
using MediatR;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Payments.Dtos;
using WorkplaceBooking.Application.Common.Models;

namespace TransactionApp.Application.Payments.Queries
{
    public class GetPaymentsWithPaginationQuery : IRequest<PaginatedList<PaymentsDto>>
    {
        public int PageNumber { get; init; } = PagedConstants.PageNumber;
        public int PageSize { get; init; } = PagedConstants.PageSize;
    }

    public class GetPaymentsWithPaginationQueryHandler : IRequestHandler<GetPaymentsWithPaginationQuery, PaginatedList<PaymentsDto>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetPaymentsWithPaginationQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<PaymentsDto>> Handle(GetPaymentsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PaginatedList<PaymentsDto>>(
                                await _repository.GetPaginated<Domain.Entities.Payments>(
                                                    request.PageNumber,
                                                    request.PageSize));
        }
    }
}
