using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.Extensions.DependencyInjection;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.Infra.Data.EF.Extensions;

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
        services.AddTransient<IGenreRepository, GenreRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork.UnitOfWork>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ICastMemberRepository, CastMemberRepository>();
    }

    private static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlixerCatalogDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("CatalogDb");
            var serverVersion = ServerVersion.AutoDetect(connectionString);
        
            options.UseMySql(connectionString, serverVersion, mysqlOptions =>
            {
                mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10), 
                    errorNumbersToAdd: null 
                );
            });

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