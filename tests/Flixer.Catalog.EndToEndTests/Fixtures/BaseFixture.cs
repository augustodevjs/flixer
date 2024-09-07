using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Flixer.Catalog.Infra.Data.EF.Context;
using Flixer.Catalog.EndToEndTests.Configuration;

namespace Flixer.Catalog.EndToEndTests.Fixtures;

public abstract class BaseFixture
{
    private readonly string _dbConnectionString;
    public ApiClient ApiClient { get; }
    public HttpClient HttpClient { get; }
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set;  }
    protected Faker Faker { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);

        var configuration = WebAppFactory.Services.GetService(typeof(IConfiguration)) as IConfiguration;
        ArgumentNullException.ThrowIfNull(configuration);
        
        _dbConnectionString = configuration.GetConnectionString("CatalogDb")!;
    }

    protected FlixerCatalogDbContext CreateDbContext()
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