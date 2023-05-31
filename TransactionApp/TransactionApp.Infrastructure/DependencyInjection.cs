using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TransactionApp.Infrastructure.Persistence;
using TransactionApp.Application.Common.Interfaces;
using TransactionApp.Infrastructure.Services;

namespace TransactionApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            //setup connection to dB
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
                configuration["ConnectionStrings:DefaultConnection"],
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
            ));

            services.AddScoped<IRepository, Repository>();

            return services;
        }
    }
}
