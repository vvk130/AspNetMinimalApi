using Microsoft.AspNetCore.Http.HttpResults;

public static class AuthorApi
{
    public static void MapAuthorApi(this WebApplication app)
    {
        app.MapGet(
            "/authors",
            async (MyDbContext context) =>
            {
                var authors = await context.Author.ToListAsync();
                return authors.Any() ? Results.Ok(authors) : Results.NotFound("No authors found.");
            }
        );
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
}
