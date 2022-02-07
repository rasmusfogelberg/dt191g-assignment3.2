using Microsoft.EntityFrameworkCore;

namespace DiscoSaurus.Data 
{
  public class DiscoSaurusContext : DbContext 
  {
    public DiscoSaurusContext(DbContextOptions<DiscoSaurusContext> options):base(options)
    {
      
    }
  }
}