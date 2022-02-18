namespace DiscoSaurus.Models
{
  public class BorrowedItem
  {
    public int BorrowedItemId { get; set; }
    public User User { get; set; }
    public Album Album { get; set; }

  }
}