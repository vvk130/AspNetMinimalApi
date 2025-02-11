public record AuthorDto(string FirstName, string LastName);

public record AuthorDtoWithId(Guid Id, string FirstName, string LastName)
    : AuthorDto(FirstName, LastName);
