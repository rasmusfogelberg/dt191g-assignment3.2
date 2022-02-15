namespace DiscoSaurus.Models
{
  public class Order
  {
    public int OrderId { get; set; }
    public DateTime OrderPlaced  { get; set; } 
    public DateTime OrderFullfilled  { get; set; }
    public ICollection<OrderItem>? OrderItems { get; set; }
  }
}