using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.IntegrationTests.Base;

public class BaseFixture
{
    public BaseFixture()
       => Faker = new Faker("pt_BR");

    protected Faker Faker { get; set; }

    public FlixerCatalogDbContext CreateDbContext(bool preserveData = false)
    {
        var context = new FlixerCatalogDbContext(
            new DbContextOptionsBuilder<FlixerCatalogDbContext>()
            .UseInMemoryDatabase("integration-tests-db")
            .Options
        );

        if (preserveData == false)
            context.Database.EnsureDeleted();

        return context;
    }
}