using AutoMapper;
using MediatR;
using TransactionApp.Application.Articles.Dtos;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;

namespace TransactionApp.Application.Articles.Commands
{
    public class UpdateArticleCommand : IRequest<ArticlesDto>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }

    public class UpdateArticleCommandHandler : IRequestHandler<UpdateArticleCommand, ArticlesDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateArticleCommandHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ArticlesDto> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _repository.GetByIdAsync<Domain.Entities.Articles>(request.Id);
            if (article == null)
            {
                throw new NotFoundException(nameof(ArticlesDto), request.Id);
            }

            article.Name = request.Name;
            article.Brand = request.Brand;
            article.Description = request.Description;
            article.Price = request.Price;
            article.StockQuantity = request.StockQuantity;

            _repository.Update(article);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ArticlesDto>(article);
        }
    }
}
