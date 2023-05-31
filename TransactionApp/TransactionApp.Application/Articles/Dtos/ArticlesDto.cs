using System.ComponentModel.DataAnnotations;
using TransactionApp.Application.Common.Mappings;
using TransactionApp.Domain.Entities;

namespace TransactionApp.Application.Articles.Dtos
{
    public class ArticlesDto : IMapFrom<Domain.Entities.Articles>
    {
        public string Name { get; set; }

        public string Brand { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public virtual ICollection<TransactionArticle> TransactionArticles { get; set; }
    }
}
