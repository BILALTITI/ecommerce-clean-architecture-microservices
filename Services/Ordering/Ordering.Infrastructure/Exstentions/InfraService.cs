using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure.Extensions
{
    public static class InfraService
    {
        public static IServiceCollection AddInfraService(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("OrderingConnectionString"),
                    sqlServerOption => sqlServerOption.EnableRetryOnFailure()
                ));

            services.AddScoped(typeof(IAsyncRepoistry<>), typeof(RepositoryBase<>));

            services.AddScoped<IOrderReposirtoy, OrderRepositry>();

            return services;
        }
    }
}
