using MediatR;
using TransactionApp.Application.Common.Interfaces;

namespace TransactionApp.Application.Customers.Commands
{
    public record DeleteCustomerCommand(Guid Id) : IRequest;

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand>
    {
        private readonly IRepository _repository;

        public DeleteCustomerCommandHandler(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            _repository.SoftDelete<Domain.Entities.Customers>(request.Id);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
