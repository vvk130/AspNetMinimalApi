using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public static class BookApi
{
    public static void MapBookApi(this WebApplication app)
    {
        app.MapPost("/books/", CreateBookForAuthor);
    }

    public static async Task<Results<Created, NotFound>> CreateBookForAuthor(
        MyDbContext context,
        [FromBody] BookRequest bookRequest
    )
    {
        var author = await context.Author.FirstOrDefaultAsync(a =>
            a.FirstName == bookRequest.AuthorDto.FirstName
            && a.LastName == bookRequest.AuthorDto.LastName
        );

        if (author is null)
            return TypedResults.NotFound();

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
}
