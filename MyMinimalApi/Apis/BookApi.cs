using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class BookApi
{
    public static void MapBookApi(this WebApplication app)
    {
        app.MapPost("/books/", CreateBookForAuthor);
        app.MapDelete("/books/", DeleteBookByName);
    }

    public static async Task<Results<Created, NotFound<ProblemDetails>>> CreateBookForAuthor(
        MyDbContext context,
        [FromBody] BookRequest bookRequest)
    {
        var author = await context.Author.FirstOrDefaultAsync(a =>
            a.FirstName == bookRequest.AuthorDto.FirstName
            && a.LastName == bookRequest.AuthorDto.LastName
        );

        if (author is null)
            return TypedResults.NotFound<ProblemDetails>(new()
            {
                Detail = "Book can only be created for an existing author"
            });

        var book = new Book()
        {
            Title = bookRequest.Title,
            Author = author,
            PublicationDate = bookRequest.PublicationDate,
            Genre = bookRequest.Genre,
        };

        context.Books.Add(book);
        await context.SaveChangesAsync();

        return TypedResults.Created();
    }

    public static async Task<Results<NoContent, NotFound>> DeleteBookByName(
        MyDbContext context,
        [FromBody] string title
    )
    {
        var book = await context.Books.FirstOrDefaultAsync(b => b.Title == title);
        if (book is null)
            return TypedResults.NotFound();

        context.Books.Remove(book);
        await context.SaveChangesAsync();
        return TypedResults.NoContent();
    }
}
