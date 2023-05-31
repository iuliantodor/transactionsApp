using AutoMapper;
using MediatR;
using TransactionApp.Application.Articles.Dtos;
using TransactionApp.Application.Common.Interfaces;

namespace TransactionApp.Application.Articles.Commands
{
    public class AddArticleCommand : IRequest<ArticlesDto>
    {
        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }

    public class AddArticleCommandHandler : IRequestHandler<AddArticleCommand, ArticlesDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public AddArticleCommandHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ArticlesDto> Handle(AddArticleCommand request, CancellationToken cancellationToken)
        {
            var article = new Domain.Entities.Articles
            {
                Name = request.Name,
                Brand = request.Brand,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
            };

            await _repository.InsertAsync(article);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ArticlesDto>(article);
        }
    }
}
