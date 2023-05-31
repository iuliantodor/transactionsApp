using TransactionApp.Application.Common.Constants;
using TransactionApp.Application.Common.Mappings;

namespace TransactionApp.Application.Payments.Dtos
{
    public class PaymentsDto : IMapFrom<Domain.Entities.Payments>
    {
        public Guid CustomerId { get; set; }

        public DateTime PaymentDate { get; set; }

        public decimal Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public int QuantitySold { get; set; }

        public Guid ArticleId { get; set; }

    }
}
