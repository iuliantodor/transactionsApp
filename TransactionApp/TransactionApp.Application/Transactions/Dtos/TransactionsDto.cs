
using TransactionApp.Domain.Entities;
using TransactionApp.Application.Common.Mappings;


namespace TransactionApp.Application.Transactions.Dtos
{
    public class TransactionsDto : IMapFrom<Domain.Entities.Transactions>
    {
        public DateTime TransactionDate { get; set; }

        public virtual ICollection<TransactionArticle> TransactionArticles { get; set; }
    }
}
