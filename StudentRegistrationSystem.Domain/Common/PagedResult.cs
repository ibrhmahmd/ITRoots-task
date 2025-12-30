namespace StudentRegistrationSystem.Domain.Common;


public interface IPagedResult
{
    int TotalCount { get; }
    int PageNumber { get; }
    int PageSize { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
    int StartRecord { get; }
    int EndRecord { get; }
}

public class PagedResult<T> : IPagedResult
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }

 
    public int PageNumber { get; set; }

    
    public int PageSize { get; set; }

    
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

    
    public bool HasPreviousPage => PageNumber > 1;

    
    public bool HasNextPage => PageNumber < TotalPages;

    
    public bool IsFirstPage => PageNumber == 1;

   
    public bool IsLastPage => PageNumber >= TotalPages;

  
    public int StartRecord => PageSize > 0 ? ((PageNumber - 1) * PageSize) + 1 : 0;

   
    public int EndRecord => Math.Min(PageNumber * PageSize, TotalCount);
}

