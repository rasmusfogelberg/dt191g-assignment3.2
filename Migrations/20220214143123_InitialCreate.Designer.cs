﻿// <auto-generated />
using DiscoSaurus.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DiscoSaurus.Migrations
{
    [DbContext(typeof(DiscoSaurusContext))]
    [Migration("20220214143123_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DiscoSaurus.Models.Album", b =>
                {
                    b.Property<int>("AlbumId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AlbumId"), 1L, 1);

                    b.Property<int>("ArtistId")
                        .HasColumnType("int");

                    b.Property<int>("GenreId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AlbumId");

                    b.HasIndex("ArtistId");

                    b.HasIndex("GenreId");

                    b.ToTable("Albums");

                    b.HasData(
                        new
                        {
                            AlbumId = 1,
                            ArtistId = 1,
                            GenreId = 1,
                            Price = 15.99m,
                            Title = "Thunderstruck"
                        },
                        new
                        {
                            AlbumId = 2,
                            ArtistId = 2,
                            GenreId = 2,
                            Price = 25.50m,
                            Title = "Balls to the Wall"
                        },
                        new
                        {
                            AlbumId = 3,
                            ArtistId = 3,
                            GenreId = 3,
                            Price = 13.60m,
                            Title = "The Number of the Beast"
                        },
                        new
                        {
                            AlbumId = 4,
                            ArtistId = 4,
                            GenreId = 1,
                            Price = 16.15m,
                            Title = "We Like It Here"
                        },
                        new
                        {
                            AlbumId = 5,
                            ArtistId = 5,
                            GenreId = 3,
                            Price = 39.99m,
                            Title = "Volume Rock"
                        });
                });

            modelBuilder.Entity("DiscoSaurus.Models.Artist", b =>
                {
                    b.Property<int>("ArtistId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ArtistId"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ArtistId");

                    b.ToTable("Artists");

                    b.HasData(
                        new
                        {
                            ArtistId = 1,
                            Name = "AC/DC"
                        },
                        new
                        {
                            ArtistId = 2,
                            Name = "Accept"
                        },
                        new
                        {
                            ArtistId = 3,
                            Name = "Iron Maiden"
                        },
                        new
                        {
                            ArtistId = 4,
                            Name = "Snarky Puppy"
                        },
                        new
                        {
                            ArtistId = 5,
                            Name = "Valley Of The Sun"
                        });
                });

            modelBuilder.Entity("DiscoSaurus.Models.Genre", b =>
                {
                    b.Property<int>("GenreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GenreId"), 1L, 1);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GenreId");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            GenreId = 1,
                            Name = "Rock"
                        },
                        new
                        {
                            GenreId = 2,
                            Name = "Jazz"
                        },
                        new
                        {
                            GenreId = 3,
                            Name = "Metal"
                        });
                });

            modelBuilder.Entity("DiscoSaurus.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            UserId = 1,
                            Name = "John Doe"
                        },
                        new
                        {
                            UserId = 2,
                            Name = "Jane Smith"
                        },
                        new
                        {
                            UserId = 3,
                            Name = "Moby Dick"
                        });
                });

            modelBuilder.Entity("DiscoSaurus.Models.Album", b =>
                {
                    b.HasOne("DiscoSaurus.Models.Artist", "Artist")
                        .WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscoSaurus.Models.Genre", "Genre")
                        .WithMany("Albums")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Artist");

                    b.Navigation("Genre");
                });

            modelBuilder.Entity("DiscoSaurus.Models.Genre", b =>
                {
                    b.Navigation("Albums");
                });
#pragma warning restore 612, 618
        }
    }
}
