using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.Extensions.DependencyInjection;

namespace Flixer.Catalog.EndToEndTests.Configuration;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("EndToEndTest");
        
        builder.ConfigureServices(services =>
        {
            var serviceProvider = services.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var context = scope.ServiceProvider.GetService<FlixerCatalogDbContext>();

            ArgumentNullException.ThrowIfNull(context);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        });

        base.ConfigureWebHost(builder);
    }
}