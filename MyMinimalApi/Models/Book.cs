public class Book
{
    public Guid Id { get; set; }
    public required string Title { get; set; }

    [ForeignKey("Id")]
    public required Author Author { get; set; }
    public DateTime PublicationDate { get; set; }
    [Required]
    public required Genre Genre { get; set; }

    public static implicit operator Book(Book v)
    {
        throw new NotImplementedException();
    }
}

public enum Genre
{
    Romance,
    Fantasy,
    Thriller
}
