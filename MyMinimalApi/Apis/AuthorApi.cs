using Microsoft.AspNetCore.Http.HttpResults;

public static class AuthorApi
{
    public static void MapAuthorApi(this WebApplication app)
    {
        app.MapGet("/authors", GetPaginatedAuthors);
        app.MapPut("/authors/{id}/address", UpdateAuthorAddress);
    }

    private static async Task<Results<Ok, NotFound>> UpdateAuthorAddress(
        MyDbContext context,
        Guid Id,
        string StreetNameAndNumberRequest
    )
    {
        var author = await context.Author.FirstOrDefaultAsync(a => a.Id == Id);

        if (author is null)
            return TypedResults.NotFound();

        author.Address = author.Address with { StreetNameAndNumber = StreetNameAndNumberRequest };
        await context.SaveChangesAsync();
        return TypedResults.Ok();
    }

    public static async Task<Ok<PaginatedList<Author>>> GetPaginatedAuthors(
        MyDbContext context,
        [AsParameters] PaginationRequest paginationRequest
    )
    {
        var size = paginationRequest.PageSize;
        var index = paginationRequest.Index;

        var authors = await context.Author.Order().Skip(size * index).Take(size).ToListAsync();

        return TypedResults.Ok(new PaginatedList<Author>(index, size, authors));
    }
}
