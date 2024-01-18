using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Flixer.Catalog.EndToEndTests.Base;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbOptions = services.FirstOrDefault(
                x => x.ServiceType == typeof(DbContextOptions<FlixerCatalogDbContext>)
            );

            if (dbOptions is not null)
                services.Remove(dbOptions);

            services.AddDbContext<FlixerCatalogDbContext>(options =>
            {
                options.UseInMemoryDatabase("e2e-tests-db");
            });
        });

        base.ConfigureWebHost(builder);
    }
}