using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.EndToEndTests.Base;

public class BaseFixture
{
    protected Faker Faker { get; set; }
    public ApiClient ApiClient { get; set; }
    public HttpClient HttpClient { get; set; }
    public CustomWebApplicationFactory<Program> WebAppFactory { get; set; }

    public BaseFixture()
    {
        Faker = new Faker("pt_BR");
        WebAppFactory = new CustomWebApplicationFactory<Program>();
        HttpClient = WebAppFactory.CreateClient();
        ApiClient = new ApiClient(HttpClient);
    }

    public FlixerCatalogDbContext CreateDbContext()
    {
        var context = new FlixerCatalogDbContext(
            new DbContextOptionsBuilder<FlixerCatalogDbContext>()
            .UseInMemoryDatabase("e2e-tests-db")
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
