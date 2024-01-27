using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.Extensions.Configuration;

namespace Flixer.Catalog.EndToEndTests.Base;

public class BaseFixture
{
    protected Faker Faker { get; set; }
    public ApiClient ApiClient { get; set; }
    public HttpClient HttpClient { get; set; }
    private readonly string _dbConnectionString;
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);

        var configuration = WebAppFactory.Services.GetService(typeof(IConfiguration));

        ArgumentNullException.ThrowIfNull(configuration);

        _dbConnectionString = ((IConfiguration)configuration).GetConnectionString("CatalogDb");
    }

    public FlixerCatalogDbContext CreateDbContext()
    {
        var context = new FlixerCatalogDbContext(
            new DbContextOptionsBuilder<FlixerCatalogDbContext>()
            .UseMySql(_dbConnectionString, ServerVersion.AutoDetect(_dbConnectionString))
            .Options
        );

        return context;
    }

    public void CleanPersistence()
    {
        var context = CreateDbContext();

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}
