using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class VideosCategoriesMapping : IEntityTypeConfiguration<VideosCategories>
{
    public void Configure(EntityTypeBuilder<VideosCategories> builder)
    {
        builder.HasKey(relation => new {
            relation.CategoryId,
            relation.VideoId
        });
    }
}