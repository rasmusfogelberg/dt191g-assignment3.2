namespace DiscoSaurus.Models
{
  public class OrderItem
  {
    public int OrderItemId { get; set; }
    public Album Album { get; set; } = new Album();
    public User User { get; set; } = new User();

  }
}