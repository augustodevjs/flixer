using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class GenreMapping : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.HasKey(genre => genre.Id);
        builder.Ignore(genre => genre.Events);
    }   
}