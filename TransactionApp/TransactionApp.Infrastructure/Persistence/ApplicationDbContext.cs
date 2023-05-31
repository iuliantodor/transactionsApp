using TransactionApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TransactionApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Customers> Customers { get; set; }

        public DbSet<Articles> Articles { get; set; }

        public DbSet<Payments> Payments { get; set; }

        public DbSet<Transactions> Transactions { get; set; }

        public DbSet<TransactionArticle> TransactionArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
