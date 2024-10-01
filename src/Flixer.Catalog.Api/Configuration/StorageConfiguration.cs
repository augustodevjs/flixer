using Flixer.Catalog.Application.Intefaces;
using Flixer.Catalog.Infra.Storage.Service;
using Flixer.Catalog.Infra.Storage.Configuration;

namespace Flixer.Catalog.Api.Configuration;

public static class StorageConfiguration
{
    public static IServiceCollection AddInfraStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AwsOptions>(configuration.GetSection("AWSConfiguration"));
        services.Configure<StorageServiceOptions>(configuration.GetSection("StorageServiceOptions"));
        
        services.AddTransient<IStorageService, StorageService>();
        
        return services;
    }
}