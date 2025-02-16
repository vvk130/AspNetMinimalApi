using System.Linq;

public static class AuthorApi
{
    public static void MapAuthorApi(this WebApplication app)
    {
        app.MapGet("/authors", GetPaginatedAuthors);
        app.MapGet("/authors/totalMoneySpent", GetTotalMoneySpentByAuthorId);
        app.MapPut("/authors/{id}/address", UpdateAuthorAddress);
    }

    private static async Task<Results<Ok, NotFound>> UpdateAuthorAddress(
        MyDbContext context,
        string firstName,
        string lastName,
        string StreetNameAndNumberRequest
    )
    {
        var author = await context
            .Author.Where(a => a.FirstName == firstName && a.LastName == lastName)
            .FirstOrDefaultAsync();

        if (author is null)
            return TypedResults.NotFound();

        author.Address = author.Address with { StreetNameAndNumber = StreetNameAndNumberRequest };
        await context.SaveChangesAsync();
        return TypedResults.Ok();
    }

    public static async Task<Ok<PaginatedList<AuthorDtoWithId>>> GetPaginatedAuthors(
        MyDbContext context,
        [AsParameters] PaginationRequest paginationRequest
    )
    {
        var size = paginationRequest.PageSize;
        var index = paginationRequest.Index;

        var authors = await context.Author.ApplyPaginationAsync(index, size);

        var finalAuthors = authors
            .Select(b => new AuthorDtoWithId(b.Id, b.FirstName, b.LastName))
            .ToList();

        return TypedResults.Ok(new PaginatedList<AuthorDtoWithId>(index, size, finalAuthors));
    }

    public static async Task<IQueryable<T>> ApplyPaginationAsync<T>(
        this IQueryable<T> query,
        int index,
        int size
    ) => query.Order().Skip(size * index).Take(size);

    private static async Task<Results<Ok<decimal>, NotFound>> GetTotalMoneySpentByAuthorId(
        MyDbContext context,
        string firstName,
        string lastName
    )
    {
        var author = await context
            .Author.Where(a => a.FirstName == firstName && a.LastName == lastName)
            .FirstOrDefaultAsync();

        if (author is null)
            return TypedResults.NotFound();

        var totalSpent = await context
            .Purchases.Where(p => p.BuyerId == author.Id)
            .SumAsync(p => p.Price);

        await context.SaveChangesAsync();
        return TypedResults.Ok(totalSpent);
    }
}
