using Catalog.API.Repositories;

namespace Catalog.API.IOC
{
    public static class RepositoriesInjection
    {
        public static IServiceCollection RepositoriesCollection(this IServiceCollection service)
            => service.AddScoped<IProductRepository, ProductRepository>();
    }
}
