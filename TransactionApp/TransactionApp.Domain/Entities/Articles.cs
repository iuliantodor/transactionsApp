using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TransactionApp.Domain.Common;

namespace TransactionApp.Domain.Entities
{
    public class Articles : BaseEntity
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be between 1 and 100 characters", MinimumLength = 1)]
        public string Name { get; set; }

        [StringLength(50, ErrorMessage = "Brand must be between 1 and 50 characters", MinimumLength = 1)]
        public string Brand { get; set; }

        [StringLength(500, ErrorMessage = "Description must be between 1 and 500 characters", MinimumLength = 1)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative number")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be a non-negative integer")]
        public int StockQuantity { get; set; }

        public virtual ICollection<TransactionArticle> TransactionArticles { get; set; }
    }
}
