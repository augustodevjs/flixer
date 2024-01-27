using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Repository;
using Microsoft.Extensions.Configuration;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Application.Contracts.UnityOfWork;

namespace Flixer.Catalog.Infra.Data.EF;

public static class DependecyInjection
{
    public static void AddInfraData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbConnection(configuration);
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUnityOfWork, UnityOfWork>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
    }

    private static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CatalogDb");

        services.AddDbContext<FlixerCatalogDbContext>(options => 
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
        );
    }
}
