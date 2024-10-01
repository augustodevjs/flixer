using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Contracts;
using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.Infra.Data.EF.UnitOfWork;
using Flixer.Catalog.Infra.Data.EF.Repositories;
using Flixer.Catalog.Domain.Contracts.Repository;

namespace Flixer.Catalog.Api.Configuration;

public static class InfrastructureConfiguration
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
        services.AddTransient<IVideoRepository, VideoRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();
        services.AddTransient<ICategoryRepository, CategoryRepository>();
        services.AddTransient<ICastMemberRepository, CastMemberRepository>();
    }

    private static void AddDbConnection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FlixerCatalogDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("CatalogDb")!;
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
        var environment = Environment
            .GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        
        if (environment == "EndToEndTest") return;
        
        using var scope = services.CreateScope();
        
        var dbContext = scope.ServiceProvider.GetRequiredService<FlixerCatalogDbContext>();
        
        dbContext.Database.Migrate();
    }
}