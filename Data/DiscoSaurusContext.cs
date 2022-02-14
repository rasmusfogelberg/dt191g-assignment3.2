using DiscoSaurus.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscoSaurus.Data 
{
  public class DiscoSaurusContext : DbContext 
  {
    public DiscoSaurusContext(DbContextOptions<DiscoSaurusContext> options):base(options)
    {
      
    }

    public DbSet<Album>? Albums { get; set; }
    public DbSet<Genre>? Genres { get; set; }
  }
}