using Microsoft.AspNetCore.Http.HttpResults;

public static class WalletApi
{
    public static void MapWalletApi(this WebApplication app)
    {
        app.MapPut("/authors/{firstName}-{lastName}/wallet", AddMoneyToWallet);
    }

    private static async Task<Results<Ok, NotFound>> AddMoneyToWallet(
        MyDbContext context,
        decimal amount,
        string firstName,
        string lastName
    )
    {
        var author = await context
            .Author.Where(a => a.FirstName == firstName && a.LastName == lastName)
            .FirstOrDefaultAsync();

        if (author is null)
            return TypedResults.NotFound();

        author.Wallet += amount;
        await context.SaveChangesAsync();
        return TypedResults.Ok();
    }
}
