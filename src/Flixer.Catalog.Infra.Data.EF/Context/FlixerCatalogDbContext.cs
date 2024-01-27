using System.Reflection;
using Flixer.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flixer.Catalog.Infra.Data.EF.Context;

public class FlixerCatalogDbContext : DbContext
{
    private object options;

    public FlixerCatalogDbContext(DbContextOptions<FlixerCatalogDbContext> options) : base(options)
    {

    }

    public FlixerCatalogDbContext(object options)
    {
        this.options = options;
    }

    public DbSet<Category> Categories { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
