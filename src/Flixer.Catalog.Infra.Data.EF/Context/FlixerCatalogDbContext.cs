﻿using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;

namespace Flixer.Catalog.Infra.Data.EF.Context;

public class FlixerCatalogDbContext : DbContext
{
    public FlixerCatalogDbContext(DbContextOptions<FlixerCatalogDbContext> options) : base(options)
    {
        
    }

    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
