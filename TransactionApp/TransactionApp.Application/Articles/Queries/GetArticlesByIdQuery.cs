using AutoMapper;
using MediatR;
using TransactionApp.Application.Articles.Dtos;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;

namespace TransactionApp.Application.Articles.Queries
{
    public class GetArticlesByIdQuery : IRequest<ArticlesDto>
    {
        public Guid Id { get; set; }
    }

    public class GetArticlesByIdQueryHandler : IRequestHandler<GetArticlesByIdQuery, ArticlesDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public GetArticlesByIdQueryHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<ArticlesDto> Handle(GetArticlesByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id == Guid.Empty)
            {
                throw new Common.Exceptions.ApplicationException("Invalid Id");
            }
            var article = await _repository.GetByIdAsync<Domain.Entities.Articles>(request.Id);

            return article != null ? _mapper.Map<ArticlesDto>(article) : throw new NotFoundException(nameof(Domain.Entities.Articles), request.Id);
        }
    }
}
