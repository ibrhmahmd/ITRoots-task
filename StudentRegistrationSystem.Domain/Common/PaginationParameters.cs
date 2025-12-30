namespace StudentRegistrationSystem.Domain.Common;

public class PaginationParameters
{
    private const int MaxPageSize = 100;
    private int _pageSize = Constants.Defaults.PageSize;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : (value < 1 ? Constants.Defaults.PageSize : value);
    }

    public string? SortBy { get; set; }

    public string SortDirection { get; set; } = "asc";

    public int Skip => (PageNumber - 1) * PageSize;

    public int Take => PageSize;

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

