using Catalog.API.Data;

namespace Catalog.API.IOC
{
    public static class DatabaseInjection
    {
        public static IServiceCollection DatabaseCollection(this IServiceCollection service)
            => service.AddScoped<ICatalogContext, CatalogContext>();
    }
}
