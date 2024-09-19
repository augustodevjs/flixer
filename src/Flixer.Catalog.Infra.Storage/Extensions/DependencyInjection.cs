using Microsoft.Extensions.Configuration;
using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Infra.Storage.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Flixer.Catalog.Infra.Storage.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<IStorageService, StorageService>();
        
        return services;
    }
}