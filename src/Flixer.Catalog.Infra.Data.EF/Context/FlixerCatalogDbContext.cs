using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Domain.Contracts;

namespace Flixer.Catalog.Infra.Data.EF.Context;

public class FlixerCatalogDbContext : DbContext, IUnityOfWork
{
    public FlixerCatalogDbContext(DbContextOptions<FlixerCatalogDbContext> options) : base(options)
    {

    }

    public DbSet<Category> Categories { get; set; } = null!;
    
    public async Task<bool> Commit() => await SaveChangesAsync() > 0;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
