public static class AuthorApi
{
    public static void MapAuthorApi(this WebApplication app)
    {
        app.MapGet("/authors", async (MyDbContext context) =>
        {
            var authors = await context.Author.ToListAsync();
            return authors.Any() ? Results.Ok(authors) : Results.NotFound("No authors found.");
        });
    }
}
