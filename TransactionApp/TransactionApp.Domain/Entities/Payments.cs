using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using TransactionApp.Application.Common.Constants;
using TransactionApp.Domain.Common;

namespace TransactionApp.Domain.Entities
{
    public class Payments : BaseEntity
    {
        public Guid CustomerId { get; set; }

        [Required(ErrorMessage = "Payment date is required")]
        public DateTime PaymentDate { get; set; }

        public Guid ArticleId { get; set; }

        [Required(ErrorMessage = "Quantity Sold is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Quantity Sold must be a non-negative number")]
        public int QuantitySold { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public PaymentStatus Status { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

    }
}
