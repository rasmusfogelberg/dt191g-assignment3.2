using DiscoSaurus.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscoSaurus.Data 
{
  public class DiscoSaurusContext : DbContext 
  {
    public DiscoSaurusContext(DbContextOptions<DiscoSaurusContext> options):base(options)
    {
    }

    public DbSet<Album> Albums { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Artist> Artists { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      builder.Entity<Genre>().HasData(
        new Genre { GenreId = 1, Name = "Rock" },
        new Genre { GenreId = 2, Name = "Jazz" },
        new Genre { GenreId = 3, Name = "Metal" }
      );

      builder.Entity<Artist>().HasData(
        new Artist { ArtistId = 1, Name = "AC/DC" },
        new Artist { ArtistId = 2, Name = "Accept" },
        new Artist { ArtistId = 3, Name = "Iron Maiden" },
        new Artist { ArtistId = 4, Name = "Snarky Puppy" },
        new Artist { ArtistId = 5, Name = "Valley Of The Sun" }
      );

      builder.Entity<Album>().HasData(
        new Album { AlbumId = 1, ArtistId = 1, GenreId = 1, Title = "Thunderstruck", Price = 15.99m },
        new Album { AlbumId = 2, ArtistId = 2, GenreId = 2, Title = "Balls to the Wall", Price = 25.50m },
        new Album { AlbumId = 3, ArtistId = 3, GenreId = 3, Title = "The Number of the Beast", Price = 13.60m },
        new Album { AlbumId = 4, ArtistId = 4, GenreId = 1, Title = "We Like It Here", Price = 16.15m },
        new Album { AlbumId = 5, ArtistId = 5, GenreId = 3, Title = "Volume Rock", Price = 39.99m }
      );

      /* builder.Entity<User>().HasData(
        new User { UserId = 1, Username = "John Doe", Password = "password" },
        new User { UserId = 2, Username = "Jane Smith", Password = "password"  },
        new User { UserId = 3, Username = "Moby Dick", Password = "password"  }
      ); */
    }
  }
}