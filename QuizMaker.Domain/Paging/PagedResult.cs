using System.Collections.Generic;

namespace QuizMaker.Domain.Paging
{
    /// <summary>
    /// Represents a paginated result set.
    /// </summary>
    /// <typeparam name="T">
    /// Type of items contained in the result.
    /// </typeparam>
    /// <remarks>
    /// Encapsulates a collection of items along with pagination metadata,
    /// including total count, page information and navigation helpers.
    /// </remarks>
    public sealed class PagedResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResult{T}"/> class.
        /// </summary>
        /// <param name="items">Items for the current page.</param>
        /// <param name="totalCount">Total number of items across all pages.</param>
        /// <param name="page">Current page number (minimum 1).</param>
        /// <param name="pageSize">Number of items per page (minimum 1).</param>
        public PagedResult(ICollection<T> items, int totalCount, int page, int pageSize)
        {
            Items = items ?? new List<T>();
            TotalCount = totalCount < 0 ? 0 : totalCount;
            Page = page < 1 ? 1 : page;
            PageSize = pageSize < 1 ? 1 : pageSize;
        }

        /// <summary>
        /// Items contained in the current page.
        /// </summary>
        public ICollection<T> Items { get; }

        /// <summary>
        /// Total number of items across all pages.
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// Current page number.
        /// </summary>
        public int Page { get; }

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Total number of available pages.
        /// </summary>
        public int TotalPages =>
            PageSize <= 0 ? 0 : (TotalCount + PageSize - 1) / PageSize;

        /// <summary>
        /// Indicates whether a previous page exists.
        /// </summary>
        public bool HasPreviousPage => Page > 1;

        /// <summary>
        /// Indicates whether a next page exists.
        /// </summary>
        public bool HasNextPage => Page < TotalPages;

        /// <summary>
        /// Gets the previous page number if available; otherwise null.
        /// </summary>
        public int? PreviousPage => HasPreviousPage ? Page - 1 : (int?)null;

        /// <summary>
        /// Gets the next page number if available; otherwise null.
        /// </summary>
        public int? NextPage => HasNextPage ? Page + 1 : (int?)null;

        /// <summary>
        /// Creates an empty paged result.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="pageSize">Page size.</param>
        /// <returns>An empty paged result instance.</returns>
        public static PagedResult<T> Empty(int page = 1, int pageSize = 50)
        {
            return new PagedResult<T>(new List<T>(), 0, page, pageSize);
        }
    }
}
