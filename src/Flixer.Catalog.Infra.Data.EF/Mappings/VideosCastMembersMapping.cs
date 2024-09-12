using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Infra.Data.EF.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class VideosCastMembersMapping : IEntityTypeConfiguration<VideosCastMembers>
{
    public void Configure(EntityTypeBuilder<VideosCastMembers> builder)
    {
        builder.HasKey(relation => new
        {
            relation.CastMemberId,
            relation.VideoId
        });
    }
}
