using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class CastMemberMapping : IEntityTypeConfiguration<CastMember>
{
    public void Configure(EntityTypeBuilder<CastMember> builder)
    {
        builder.HasKey(castMember => castMember.Id);

        builder.Property(castMember => castMember.Name)
            .HasMaxLength(255);
    }
}