public class BookRequestValidator : AbstractValidator<BookRequest>
{
    public BookRequestValidator()
    {
        string titlePattern = @"^[a-zA-ZåäöÅÄÖ\s]+$";

        RuleFor(b => b.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .Matches(titlePattern)
            .WithMessage("Title can only contain letters (a-z, A-Z, å, ä, ö)");

        RuleFor(b => b.Genre).IsInEnum().WithMessage("Genre needs to be between 0-2");

        RuleFor(b => b.Stock)
            .InclusiveBetween(0,100)
            .WithMessage("Stock needs to be between 0-100");
    }
}
