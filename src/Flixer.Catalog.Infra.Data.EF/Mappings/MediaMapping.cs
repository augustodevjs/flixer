using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class MediaMapping : IEntityTypeConfiguration<Media>
{
    public void Configure(EntityTypeBuilder<Media> builder)
    {
        builder.Property(media => media.Id).ValueGeneratedNever();
    }
}