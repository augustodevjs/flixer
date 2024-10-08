﻿using Microsoft.EntityFrameworkCore;
using Flixer.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Flixer.Catalog.Infra.Data.EF.Mappings;

public class VideoMapping  : IEntityTypeConfiguration<Video> 
{
    public void Configure(EntityTypeBuilder<Video> builder)
    {
        builder.HasKey(video => video.Id);

        builder.Navigation(x => x.Media).AutoInclude();
        builder.Navigation(x => x.Trailer).AutoInclude();
        
        builder.Property(video => video.Id).ValueGeneratedNever();
        builder.Property(video => video.Title).HasMaxLength(255);
        builder.Property(video => video.Description).HasMaxLength(4_000);
        
        builder.OwnsOne(video => video.Thumb, thumb =>
            thumb.Property(image => image.Path).HasColumnName("ThumbPath")
        );
        
        builder.OwnsOne(video => video.ThumbHalf, thumbHalf =>
            thumbHalf.Property(image => image.Path).HasColumnName("ThumbHalfPath")
        );
        
        builder.OwnsOne(video => video.Banner, banner =>
            banner.Property(image => image.Path).HasColumnName("bannerPath")
        );

        builder.HasOne(video => video.Media)
            .WithOne()
            .HasForeignKey<Video>("MediaId");
        
        builder.HasOne(video => video.Trailer)
            .WithOne()
            .HasForeignKey<Video>("TrailerId");

        builder.Property<DateTime>("LastUpdated")
            .ValueGeneratedOnAdd();
        
        builder.Ignore(video => video.Events);
    }
}
