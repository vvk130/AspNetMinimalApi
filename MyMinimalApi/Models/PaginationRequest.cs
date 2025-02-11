public record PaginationRequest(int PageSize = 10, int Index = 0)
{
    public int PageSize { get; init; } = PageSize > 100 ? 100 : PageSize;
}