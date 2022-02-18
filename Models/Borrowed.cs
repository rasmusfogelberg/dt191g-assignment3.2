namespace DiscoSaurus.Models
{
  public class Borrowed
  {
    public int BorrowedId { get; set; }
    public DateTime BorrowedAt  { get; set; } 
    public DateTime? ReturnedAt  { get; set; }
    public BorrowedItem BorrowedItem { get; set; }
  }
}