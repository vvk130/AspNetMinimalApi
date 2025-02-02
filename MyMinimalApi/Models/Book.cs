public class Book
{
    public Guid Id { get; set; } = new();
    public required string Title { get; set; }
    public required Author Author { get; set; }
    public required DateTime PublicationDate { get; set; }
    [Required]
    public required Genre Genre { get; set; }
}

public enum Genre
{
    Romance,
    Fantasy,
    Thriller
}
