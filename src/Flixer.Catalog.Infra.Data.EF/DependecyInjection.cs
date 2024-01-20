using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Repository;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Application.Contracts.UnityOfWork;

namespace Flixer.Catalog.Infra.Data.EF;

public static class DependecyInjection
{
    public static void AddInfraData(this IServiceCollection services)
    {
        services.AddDbConnection();
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IUnityOfWork, UnityOfWork>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
    }

    private static void AddDbConnection(this IServiceCollection services)
    {
        services.AddDbContext<FlixerCatalogDbContext>(options => 
            options.UseInMemoryDatabase("InMemory-DSB-Database")
        );
    }
}
