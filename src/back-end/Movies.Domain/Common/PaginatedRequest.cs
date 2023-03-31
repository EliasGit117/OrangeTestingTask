namespace Movies.Domain.Common;

public class PaginatedRequest
{
    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;

    public int Page { get; set; } = 1;
    
    private int _itemsPerPage = DefaultPageSize;
    public int ItemsPerPage
    {
        get => _itemsPerPage;
        set => _itemsPerPage = (value > MaxPageSize) ? MaxPageSize : value;
    }
}