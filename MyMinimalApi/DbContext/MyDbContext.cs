public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Author { get; set; }
    public DbSet<BookReview> BookReviews { get; set; }
    public DbSet<Purchase> Purchases { get; set; }
}
