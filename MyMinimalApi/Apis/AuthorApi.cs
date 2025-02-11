public static class AuthorApi
{
    public static void MapAuthorApi(this WebApplication app)
    {
        app.MapGet("/authors", GetPaginatedAuthors);
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

        var authors = await context
            .Author
            .OrderBy(b => b.LastName)
            .Select(b => new AuthorDtoWithId(b.Id, b.FirstName, b.LastName))
            .Skip(size * index)
            .Take(size)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedList<AuthorDtoWithId>(index, size, authors));
    }

}
