namespace MyWebApi.App.Services;

/// <summary>
/// A wrapper to return paged results and metadata for the imaginary frontend
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Data">Entities</param>
/// <param name="TotalCount">Total number of results</param>
/// <param name="CurrentPage"></param>
/// <param name="PageSize">How many entities per page</param>
public record PagedResult<T>
(
    IEnumerable<T> Data, 
    int TotalCount, 
    int CurrentPage, 
    int PageSize)
{
    /// <summary>
    /// Integers cant hold floating-point numbers. So simple integer divison will eventually return incorrect pages (ex. 2.9 = 2).
    /// DivRem uses euclidean division to return the remainder sum too, and deals with integer overflow.
    /// A more performant alternative would be to use: (TotalCount + PageSize - 1) / PageSize (and logic for integer overflow)
    /// But I chose to use this method, since the gain in readability outweighs negligible loss of performance.
    /// </summary>
    public int TotalPages
    {
        get
        {
            (var quotient, var remainder) = Math.DivRem(TotalCount, PageSize);
            return remainder > 0 ? quotient+1 : quotient;
        }
    } 

    /// <summary>
    /// Basic logic to make sure if we can navigate forward/backwards in the data
    /// </summary>
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;
}

