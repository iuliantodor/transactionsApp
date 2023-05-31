using System.ComponentModel.DataAnnotations;
using TransactionApp.Domain.Common;

namespace TransactionApp.Domain.Entities
{
    public class Customers : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters", MinimumLength = 1)]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [StringLength(100, ErrorMessage = "Address must be between 1 and 100 characters", MinimumLength = 1)]
        public string Address { get; set; }
    }
}
