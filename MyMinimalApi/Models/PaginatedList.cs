public class PaginatedList<TEntity>(int index, int pageSize, IEnumerable<TEntity> data) where TEntity : class
{
    public int Index { get; } = index;

    public int PageSize { get; } = pageSize;

    public IEnumerable<TEntity> Data { get;} = data;
}