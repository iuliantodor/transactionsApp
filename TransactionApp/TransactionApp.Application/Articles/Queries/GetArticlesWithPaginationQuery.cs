using AutoMapper;
using MediatR;
using TransactionApp.Application.Articles.Dtos;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Interfaces;
using WorkplaceBooking.Application.Common.Models;

namespace TransactionApp.Application.Articles.Queries
{
    public class GetArticlesWithPaginationQuery : IRequest<PaginatedList<ArticlesDto>>
    {
        public int PageNumber { get; init; } = PagedConstants.PageNumber;
        public int PageSize { get; init; } = PagedConstants.PageSize;
    }

    public class GetArticlesWithPaginationQueryHandler : IRequestHandler<GetArticlesWithPaginationQuery, PaginatedList<ArticlesDto>>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetArticlesWithPaginationQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ArticlesDto>> Handle(GetArticlesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<PaginatedList<ArticlesDto>>(
                                await _repository.GetPaginated<Domain.Entities.Articles>(
                                                    request.PageNumber,
                                                    request.PageSize));
        }
    }
}

