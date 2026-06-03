namespace Coincap.Share;

// Envuelve una lista paginada con los datos
public class PagedResult<T>
{
    public IEnumerable<T> Data { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int Total { get; }

    // TotalPages
    public int TotalPages => (int)Math.Ceiling(Total / (double)PageSize);

    public PagedResult(IEnumerable<T> data, int total, int page, int pageSize)
    {
        Data = data;
        Total = total;
        Page = page;
        PageSize = pageSize;
    }
}
