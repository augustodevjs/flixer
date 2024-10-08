﻿using Bogus;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.IntegrationTests.Fixtures;

public abstract class BaseFixture
{
    public Faker Faker { get; set; }
    
    protected BaseFixture() => Faker = new Faker("pt_BR");
    
    public FlixerCatalogDbContext CreateDbContext(string nameDataBase, bool preserveData = false)
    {
        var context = new FlixerCatalogDbContext(
            new DbContextOptionsBuilder<FlixerCatalogDbContext>()
                .UseInMemoryDatabase(nameDataBase)
                .Options
        );

        if (preserveData == false)
            context.Database.EnsureDeleted();

        return context;
    }
}