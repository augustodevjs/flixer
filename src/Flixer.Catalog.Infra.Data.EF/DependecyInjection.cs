using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.Infra.Data.EF;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRepositories();
        services.AddDbConnection(configuration);

        return services;
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<ICategoryRepository, CategoryRepository>();
    }

    private static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlixerCatalogDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("CatalogDb");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
            options.UseMySql(connectionString, serverVersion);
            options.EnableDetailedErrors();
            options.EnableSensitiveDataLogging();
        });
    }
    
    public static void UseMigrations(this IApplicationBuilder app, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FlixerCatalogDbContext>();
        db.Database.Migrate();
    }
}