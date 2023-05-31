using MediatR;
using TransactionApp.Application.Common.Interfaces;

namespace TransactionApp.Application.Payments.Commands
{
    public record DeletePaymentCommand(Guid Id) : IRequest;
    public class DeletePaymentCommandHandler : IRequestHandler<DeletePaymentCommand>
    {
        private readonly IRepository _repository;

        public DeletePaymentCommandHandler(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
        {
            _repository.SoftDelete<Domain.Entities.Payments>(request.Id);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
