var builder = WebApplication.CreateBuilder(args);

var dbUrl = builder.Configuration.GetConnectionString("DefaultConnection");

builder.AddFluentValidationEndpointFilter();

builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(dbUrl));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseStatusCodePages();

app.MapBookApi();
app.MapAuthorApi();
app.MapWalletApi();
app.MapPurchaseApi();

app.UseResponseCaching();

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
        {
            Public = true,
            MaxAge = TimeSpan.FromSeconds(7)
        };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
        new string[] { "Accept-Encoding" };

    await next();
});


app.Run();
