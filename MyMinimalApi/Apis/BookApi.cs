using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public static class BookApi
{
    public static void MapBookApi(this WebApplication app)
    {
        app.MapPost("/books/", CreateBookForAuthor).AddFluentValidationFilter();
        app.MapDelete("/books/", DeleteBookByName)
            .WithName("DeleteBookByTitle")
            .WithSummary("Give a book title to delete it");
        app.MapGet("/books/", GetPaginatedBooks);
    }

    public static async Task<Results<Created, NotFound<ProblemDetails>>> CreateBookForAuthor(
        [FromBody] BookRequest bookRequest,
        MyDbContext context,
        IValidator<BookRequest> validator
    )
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
            Stock = bookRequest.Stock,
            Price = bookRequest.Price,
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

    public static async Task<Ok<PaginatedList<BookDto>>> GetPaginatedBooks(
        MyDbContext context,
        [AsParameters] PaginationRequest paginationRequest
    )
    {
        var size = paginationRequest.PageSize;
        var index = paginationRequest.Index;

        var books = await context
            .Books
            .OrderBy(b => b.Title)
            .Select(b => new BookDto(b.Id, b.Title))
            .Skip(size * index)
            .Take(size)
            .ToListAsync();

        return TypedResults.Ok(new PaginatedList<BookDto>(index, size, books));
    }

}
