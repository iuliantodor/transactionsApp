using TransactionApp.Domain.Common;

namespace TransactionApp.Domain.Entities
{
    public class TransactionArticle : BaseEntity
    {
        public Transactions Transaction { get; set; }
        public Articles Articles { get; set; }
    }
}
