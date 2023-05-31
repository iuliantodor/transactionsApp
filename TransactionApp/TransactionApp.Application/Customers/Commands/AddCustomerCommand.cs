using AutoMapper;
using MediatR;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Application.Customers.Dtos;

namespace TransactionApp.Application.Customers.Commands
{
    public class AddCustomerCommand : IRequest<CustomersDto>
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }

    public class AddCustomerCommandHandler : IRequestHandler<AddCustomerCommand, CustomersDto>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public AddCustomerCommandHandler(
           IRepository repository,
           IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CustomersDto> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Domain.Entities.Customers
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address
            };

            await _repository.InsertAsync(customer);
            await _repository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CustomersDto>(customer);
        }
    }
}
