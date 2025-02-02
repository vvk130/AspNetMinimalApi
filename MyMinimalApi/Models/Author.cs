[Index(nameof(FirstName), nameof(LastName), IsUnique = true)]
public class Author
{
    public Guid Id { get; set; } = new();

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required Address Address { get; set; }

    public string? Biography { get; set; }

    public List<Book> Books { get; set; } = [];
}

[ComplexType]
public class Address
{
    [Required]
    [MaxLength(100)]
    public required string StreetNameAndNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    [Required]
    [MinLength(5)]
    [MaxLength(5)]
    public required string PostalCode { get; set; }

    [Required]
    public required string City { get; set; }
}
