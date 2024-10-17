using HealthChecks.UI.Client;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Flixer.Catalog.Api.Configuration;

public static class HealthChecksConfiguration
{
    public static void ConfigureApplicationHealthChecks(this IHealthChecksBuilder builder, IConfiguration configuration)
    {
        var catalogDb = configuration.GetConnectionString("CatalogDb")!;
        
        builder
            .AddMySql(catalogDb, "SELECT 1")
            .AddDbContextCheck<FlixerCatalogDbContext>("FlixerDb");
    }
    
    public static void UseApplicationHealthCheck(this WebApplication app)
    {
        app.UseHealthChecks("/health-check", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });
    }
}