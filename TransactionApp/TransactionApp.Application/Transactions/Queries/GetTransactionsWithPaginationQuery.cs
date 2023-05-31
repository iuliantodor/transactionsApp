using AutoMapper;
using MediatR;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Transactions.Dtos;
using WorkplaceBooking.Application.Common.Models;

namespace TransactionApp.Application.Transactions.Queries
{
    public class GetTransactionsWithPaginationQuery : IRequest<PaginatedList<TransactionsDto>>
    {
        public int PageNumber { get; init; } = PagedConstants.PageNumber;
        public int PageSize { get; init; } = PagedConstants.PageSize;
    }

    public class GetTransactionsWithPaginationQueryHandler : IRequestHandler<GetTransactionsWithPaginationQuery, PaginatedList<TransactionsDto>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetTransactionsWithPaginationQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<TransactionsDto>> Handle(GetTransactionsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PaginatedList<TransactionsDto>>(
                                await _repository.GetPaginated<Domain.Entities.Transactions>(
                                                    request.PageNumber,
                                                    request.PageSize));
        }
    }
}
