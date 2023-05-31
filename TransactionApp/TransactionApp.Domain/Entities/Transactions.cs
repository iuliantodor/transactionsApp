using TransactionApp.Domain.Common;

namespace TransactionApp.Domain.Entities
{
    public class Transactions : BaseEntity
    {
        public DateTime TransactionDate { get; set; }

        public virtual ICollection<TransactionArticle> TransactionArticles { get; set; }

    }
}
