using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Flixer.Catalog.Infra.Data.EF.Models;

namespace Flixer.Catalog.Infra.Data.EF.Context;

public class FlixerCatalogDbContext : DbContext
{
    public FlixerCatalogDbContext(DbContextOptions<FlixerCatalogDbContext> options) : base(options)
    {

    }

    public DbSet<Media> Medias { get; set; } = null!;
    public DbSet<Genre> Genres { get; set; } = null!;
    public DbSet<Video> Videos { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<CastMember> CastMembers { get; set; } = null!;
    
    public DbSet<VideosGenres> VideosGenres { get; set; } = null!;
    public DbSet<GenresCategories> GenresCategories { get; set; } = null!;
    public DbSet<VideosCategories> VideosCategories { get; set; } = null!;
    public DbSet<VideosCastMembers> VideosCastMembers { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
