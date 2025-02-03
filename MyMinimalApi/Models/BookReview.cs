[Index(nameof(Id), IsUnique = true)]
public class BookReview
{
    public required Guid Id { get; set; } = new();

    public required string ReviewText { get; set; }

    [Range(1, 5)]
    public required int Rating { get; set; }

    public required Book Book { get; set; }

    public required DateOnly Created = DateOnly.FromDateTime(DateTime.Now);
}
