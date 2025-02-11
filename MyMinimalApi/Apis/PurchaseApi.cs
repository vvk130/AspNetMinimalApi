public static class PurchaseApi
{
    public static void MapPurchaseApi(this WebApplication app)
    {
        app.MapPost("/authors/{bookId}/purchase", BuyBookById).AddFluentValidationFilter();
        app.MapGet("/purchases/", GetPaginatedPurchases);
    }

    public static async Task<Ok<PaginatedList<Purchase>>> GetPaginatedPurchases(
        MyDbContext context,
        [AsParameters] PaginationRequest paginationRequest
    )
    {
        var size = paginationRequest.PageSize;
        var index = paginationRequest.Index;

        var purchases = await context.Purchases.Order().Skip(size * index).Take(size).ToListAsync();

        return TypedResults.Ok(new PaginatedList<Purchase>(index, size, purchases));
    }

    private static async Task<Results<Created, NotFound<ProblemDetails>>> BuyBookById(
        MyDbContext context,
        Guid bookId,
        string firstName,
        string lastName
    )
    {
        var book = await context
            .Books.Select(b => new
            {
                Id = b.Id,
                Stock = b.Stock,
                Price = b.Price
            })
            .Where(b => b.Id == bookId)
            .FirstOrDefaultAsync();

        if (book == null)
            return TypedResults.NotFound<ProblemDetails>(
                new() { Detail = "Book with given id doesn't exist" }
            );

        if (book.Stock <= 0)
            return TypedResults.NotFound<ProblemDetails>(new() { Detail = "Book is out of stock" });

        var author = await context
            .Author.Select(a => new
            {
                a.Id,
                a.Wallet,
                a.FirstName,
                a.LastName
            })
            .Where(a => a.FirstName == firstName && a.LastName == lastName)
            .FirstOrDefaultAsync();

        if (author is null)
            return TypedResults.NotFound<ProblemDetails>(
                new() { Detail = "Author with given id doesn't exist" }
            );

        if (author.Wallet < book.Price)
            return TypedResults.NotFound<ProblemDetails>(
                new() { Detail = "Author doesn't have enough money in their wallet" }
            );

        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await context
                .Books.Where(b => b.Id == bookId && b.Stock >= 1)
                .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Stock, book.Stock - 1));
            await context
                .Author.Where(a => a.Id == author.Id && a.Wallet >= book.Price)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(a => a.Wallet, author.Wallet - book.Price)
                );

            var purchase = new Purchase()
            {
                BookId = book.Id,
                BuyerId = author.Id,
                Price = book.Price,
            };

            context.Purchases.Add(purchase);
            await context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            return TypedResults.NotFound<ProblemDetails>(new() { Detail = "Transaction failed" });
        }
        return TypedResults.Created();
    }
}
