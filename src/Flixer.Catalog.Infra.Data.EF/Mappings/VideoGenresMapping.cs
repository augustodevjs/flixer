using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class VideoGenresMapping : IEntityTypeConfiguration<VideosGenres>
{
    public void Configure(EntityTypeBuilder<VideosGenres> builder)
    {
        builder.HasKey(relation => new
        {
            relation.GenreId,
            relation.VideoId
        });
    }
}