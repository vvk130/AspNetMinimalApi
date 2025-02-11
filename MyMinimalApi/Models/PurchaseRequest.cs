public record PurchaseRequest(
    Guid AuthorId,
    string AuthorFirstName,
    string AuthorLastName,
    decimal Wallet,
    Guid BookId,
    int Stock,
    decimal Price
);
