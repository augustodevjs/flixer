using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class GenresCategoriesMapping : IEntityTypeConfiguration<GenresCategories>
{
    public void Configure(EntityTypeBuilder<GenresCategories> builder)
    {
        builder.HasKey(relation => new
        {
            relation.GenreId,
            relation.CategoryId
        });
    }
}