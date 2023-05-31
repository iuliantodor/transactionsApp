using TransactionApp.Application.Common.Mappings;

namespace TransactionApp.Application.Customers.Dtos
{
    public class CustomersDto : IMapFrom<Domain.Entities.Customers>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
