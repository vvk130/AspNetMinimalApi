API live: [http://netcoreapp.runasp.net/swagger/index.html](http://netcoreapp.runasp.net/swagger/index.html)

New EF Core features featured

- Complex types (Author Address Update endpoint)
- Automatic ordering by primary key (Order()) (Paginated Response)

Other features utilized:

- Using Database Transactions for payments
- Using library to provide Validation Errors combatible with TypedResults: https://github.com/Carl-Hugo/FluentValidation.AspNetCore.Http
- FluentValidation (So I started using the MVC errors and changed to Fluent, so both are featured!)
- Response caching (7 sec)
- Immutable classes used as paginated responses (Optimal choice to ensure thredsafe caching), otherwise utilizing records as dto
- Manual dto mapping (with Select statement to prevent bloating the payload)
