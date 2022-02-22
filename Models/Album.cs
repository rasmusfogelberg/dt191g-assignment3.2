namespace DiscoSaurus.Models
{
  public class Album
  {
    public int AlbumId { get; set; }
    public int GenreId { get; set; }
    public int ArtistId { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int? LentToUser { get; set; }
    public Genre Genre { get; set; }
    public Artist Artist { get; set; }
  }
}