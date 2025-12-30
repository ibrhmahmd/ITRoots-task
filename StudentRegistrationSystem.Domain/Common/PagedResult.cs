namespace StudentRegistrationSystem.Domain.Common;

/// <summary>
/// Non-generic interface for pagination to support covariance
/// </summary>
public interface IPagedResult
{
    int TotalCount { get; }
    int PageNumber { get; }
    int PageSize { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
    bool IsFirstPage { get; }
    bool IsLastPage { get; }
    int StartRecord { get; }
    int EndRecord { get; }
}

/// <summary>
/// Generic paginated result model
/// </summary>
/// <typeparam name="T">The type of items in the result</typeparam>
public class PagedResult<T> : IPagedResult
{
    /// <summary>
    /// The items for the current page
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Total number of records across all pages
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Current page number (1-based)
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Number of items per page
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total number of pages
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

    /// <summary>
    /// Indicates if there is a previous page
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Indicates if there is a next page
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Indicates if this is the first page
    /// </summary>
    public bool IsFirstPage => PageNumber == 1;

    /// <summary>
    /// Indicates if this is the last page
    /// </summary>
    public bool IsLastPage => PageNumber >= TotalPages;

    /// <summary>
    /// Starting record number (1-based)
    /// </summary>
    public int StartRecord => PageSize > 0 ? ((PageNumber - 1) * PageSize) + 1 : 0;

    /// <summary>
    /// Ending record number (1-based)
    /// </summary>
    public int EndRecord => Math.Min(PageNumber * PageSize, TotalCount);
}

