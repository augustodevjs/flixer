using Microsoft.Extensions.Configuration;
using Flixer.Catalog.Infra.Storage.Service;
using Flixer.Catalog.Application.Intefaces;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Infra.Storage.Configuration;

namespace Flixer.Catalog.Infra.Storage.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsOptions>(configuration.GetSection("AWSConfiguration"));
        services.Configure<StorageServiceOptions>(configuration.GetSection("StorageServiceOptions"));
        
        services.AddTransient<IStorageService, StorageService>();
        
        return services;
    }
}