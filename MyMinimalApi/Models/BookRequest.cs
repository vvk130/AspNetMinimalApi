public record BookRequest(
    string Title,
    AuthorDto AuthorDto,
    DateTime PublicationDate,
    Genre Genre,
    int Stock,
    decimal Price
);
