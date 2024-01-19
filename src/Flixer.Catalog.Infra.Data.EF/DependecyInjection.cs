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
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<IUnityOfWork, UnityOfWork>();
    }
}
