using MyWebApi.Domain.Entities;

namespace MyWebApi.App.Querying;

/// <summary>
/// I extend the <see cref="IQueryable{T}"/>  interface to implement some own logic 
/// for search, pagination etc. Before we would use "this" to signal that its
/// an extension, but this is a newer way.
/// IQueryable adds the instructions as nodes to a Expression Tree instead of running anything. So its a smart
/// way to build our own advanced queries. By building the query first, we practically turn our seach into a lazy search.
/// </summary>
public static class IQueryExtensions
{


    extension<T>(IQueryable<T> query)
    {
        /// <summary>
        /// Simple pagination
        /// </summary>
        /// <param name="currentPage">Current datapoint to go from</param>
        /// <param name="pageSize">steps to next datapoint</param>
        /// <returns></returns>
        public IQueryable<T> ApplyPagination(int currentPage, int pageSize)
        {
            currentPage = currentPage < 1 ? 1 : currentPage;
            pageSize = pageSize < 1 ? 10 : Math.Min(pageSize, 100);
            var toSkip = (currentPage - 1) * pageSize;
            return query
                .Skip(toSkip)
                .Take(pageSize);
        }
    }


    extension(IQueryable<Workspace> query)
    {
        public IQueryable<Workspace> ForUser(Guid userId)
        {
            return query.Where(w =>
                w.OwnerId == userId ||
                w.Members.Any(m => m.UserId == userId)
            );
        }
    }

}

