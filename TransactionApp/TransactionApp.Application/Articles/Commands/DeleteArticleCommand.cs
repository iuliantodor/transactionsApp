using MediatR;
using TransactionApp.Application.Common.Interfaces;

namespace TransactionApp.Application.Articles.Commands
{
    public record DeleteArticleCommand(Guid Id) : IRequest;

    public class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand>
    {
        private readonly IRepository _repository;

        public DeleteArticleCommandHandler(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            _repository.SoftDelete<Domain.Entities.Articles>(request.Id);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
