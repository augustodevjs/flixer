﻿using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Context;

namespace Flixer.Catalog.IntegrationTests.Infra.Data.EF.Base;

public class BaseFixture
{
    protected Faker Faker {  get; set; }

    public BaseFixture()
        => Faker = new Faker("pt_BR");

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