[Index(nameof(Id), IsUnique = true)]
public class Purchase
{
    public Guid Id { get; } = new();
    public required Guid BuyerId { get; set; }
    public required Guid BookId { get; set; }
    public required decimal Price { get; set; }
}
