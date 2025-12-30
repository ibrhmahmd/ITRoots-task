namespace StudentRegistrationSystem.Domain.Common;

/// <summary>
/// Parameters for pagination requests
/// </summary>
public class PaginationParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = Constants.Defaults.PageSize;

    /// <summary>
    /// Page number (1-based). Default is 1.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Number of items per page. Default is 10. Maximum is 100.
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? Constants.Defaults.PageSize : value);
    }

    /// <summary>
    /// Column name to sort by
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction: "asc" or "desc". Default is "asc".
    /// </summary>
    public string SortDirection { get; set; } = "asc";

    /// <summary>
    /// Calculates the number of records to skip (for OFFSET clause)
    /// </summary>
    public int Skip => (PageNumber - 1) * PageSize;

    /// <summary>
    /// Gets the number of records to take (for FETCH NEXT clause)
    /// </summary>
    public int Take => PageSize;

    /// <summary>
    /// Validates and normalizes the pagination parameters
    /// </summary>
    public void Validate()
    {
        if (PageNumber < 1)
            PageNumber = 1;

        if (PageSize < 1)
            PageSize = Constants.Defaults.PageSize;

        if (PageSize > MaxPageSize)
            PageSize = MaxPageSize;

        if (string.IsNullOrWhiteSpace(SortDirection))
            SortDirection = "asc";
        else
            SortDirection = SortDirection.ToLower() == "desc" ? "desc" : "asc";
    }
}

