﻿// <auto-generated />
using System;
using Flixer.Catalog.Infra.Data.EF.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Flixer.Catalog.Infra.Data.EF.Migrations
{
    [DbContext(typeof(FlixerCatalogDbContext))]
    [Migration("20240912141255_addVideo")]
    partial class addVideo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Flixer.Catalog.Domain.Entities.CastMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("CastMembers");
                });

            modelBuilder.Entity("Flixer.Catalog.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("varchar(10000)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Flixer.Catalog.Domain.Entities.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Flixer.Catalog.Domain.Entities.Media", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<string>("EncodedPath")
                        .HasColumnType("longtext");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Media");
                });

            modelBuilder.Entity("Flixer.Catalog.Domain.Entities.Video", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("varchar(4000)");

                    b.Property<int>("Duration")
                        .HasColumnType("int");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("MediaId")
                        .HasColumnType("char(36)");

                    b.Property<bool>("Opened")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("Published")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<Guid?>("TrailerId")
                        .HasColumnType("char(36)");

                    b.Property<int>("YearLaunched")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MediaId")
                        .IsUnique();

                    b.HasIndex("TrailerId")
                        .IsUnique();

                    b.ToTable("Videos");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.GenresCategories", b =>
                {
                    b.Property<Guid>("GenreId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.HasKey("GenreId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("GenresCategories");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.VideosCastMembers", b =>
                {
                    b.Property<Guid>("CastMemberId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("char(36)");

                    b.HasKey("CastMemberId", "VideoId");

                    b.HasIndex("VideoId");

                    b.ToTable("VideosCastMembers");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.VideosCategories", b =>
                {
                    b.Property<Guid>("CategoryId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("char(36)");

                    b.HasKey("CategoryId", "VideoId");

                    b.HasIndex("VideoId");

                    b.ToTable("VideosCategories");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.VideosGenres", b =>
                {
                    b.Property<Guid>("GenreId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("VideoId")
                        .HasColumnType("char(36)");

                    b.HasKey("GenreId", "VideoId");

                    b.HasIndex("VideoId");

                    b.ToTable("VideosGenres");
                });

            modelBuilder.Entity("Flixer.Catalog.Domain.Entities.Video", b =>
                {
                    b.HasOne("Flixer.Catalog.Domain.Entities.Media", "Media")
                        .WithOne()
                        .HasForeignKey("Flixer.Catalog.Domain.Entities.Video", "MediaId");

                    b.HasOne("Flixer.Catalog.Domain.Entities.Media", "Trailer")
                        .WithOne()
                        .HasForeignKey("Flixer.Catalog.Domain.Entities.Video", "TrailerId");

                    b.OwnsOne("Flixer.Catalog.Domain.ValueObject.Image", "Banner", b1 =>
                        {
                            b1.Property<Guid>("VideoId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Path")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("bannerPath");

                            b1.HasKey("VideoId");

                            b1.ToTable("Videos");

                            b1.WithOwner()
                                .HasForeignKey("VideoId");
                        });

                    b.OwnsOne("Flixer.Catalog.Domain.ValueObject.Image", "Thumb", b1 =>
                        {
                            b1.Property<Guid>("VideoId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Path")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("ThumbPath");

                            b1.HasKey("VideoId");

                            b1.ToTable("Videos");

                            b1.WithOwner()
                                .HasForeignKey("VideoId");
                        });

                    b.OwnsOne("Flixer.Catalog.Domain.ValueObject.Image", "ThumbHalf", b1 =>
                        {
                            b1.Property<Guid>("VideoId")
                                .HasColumnType("char(36)");

                            b1.Property<string>("Path")
                                .IsRequired()
                                .HasColumnType("longtext")
                                .HasColumnName("ThumbHalfPath");

                            b1.HasKey("VideoId");

                            b1.ToTable("Videos");

                            b1.WithOwner()
                                .HasForeignKey("VideoId");
                        });

                    b.Navigation("Banner");

                    b.Navigation("Media");

                    b.Navigation("Thumb");

                    b.Navigation("ThumbHalf");

                    b.Navigation("Trailer");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.GenresCategories", b =>
                {
                    b.HasOne("Flixer.Catalog.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flixer.Catalog.Domain.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.VideosCastMembers", b =>
                {
                    b.HasOne("Flixer.Catalog.Domain.Entities.CastMember", "CastMember")
                        .WithMany()
                        .HasForeignKey("CastMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flixer.Catalog.Domain.Entities.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CastMember");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.VideosCategories", b =>
                {
                    b.HasOne("Flixer.Catalog.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flixer.Catalog.Domain.Entities.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Flixer.Catalog.Infra.Data.EF.Models.VideosGenres", b =>
                {
                    b.HasOne("Flixer.Catalog.Domain.Entities.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Flixer.Catalog.Domain.Entities.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Video");
                });
#pragma warning restore 612, 618
        }
    }
}
