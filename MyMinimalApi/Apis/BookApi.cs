public static class BookApi
{
    public static void MapBookApi(this WebApplication app)
    {
        app.MapGet("/", () => "Hello World!");
    }
}
