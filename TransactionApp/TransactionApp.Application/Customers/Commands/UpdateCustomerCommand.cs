using AutoMapper;
using MediatR;
using TransactionApp.Application.Common.Exceptions;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Customers.Dtos;

namespace TransactionApp.Application.Customers.Commands
{
    public class UpdateCustomerCommand : IRequest<CustomersDto>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomersDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public UpdateCustomerCommandHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomersDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _repository.GetByIdAsync<Domain.Entities.Customers>(request.Id);
            if (customer == null)
            {
                throw new NotFoundException(nameof(CustomersDto), request.Id);
            }

            customer.Name = request.Name;
            customer.Email = request.Email;
            customer.PhoneNumber = request.PhoneNumber;
            customer.Address = request.Address;

            _repository.Update(customer);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CustomersDto>(customer);
        }
    }
}
