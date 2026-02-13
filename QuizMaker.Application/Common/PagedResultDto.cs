using System.Collections.Generic;

namespace QuizMaker.Application.Common
{
    /// <summary>
    /// Represents a paged response returned from the API.
    /// </summary>
    /// <typeparam name="T">
    /// Type of items contained in the result set.
    /// </typeparam>
    /// <remarks>
    /// This object contains pagination metadata along with the requested data page.
    /// </remarks>
    public class PagedResultDto<T>
    {
        /// <summary>
        /// Collection of items for the current page.
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// Total number of items available for the given query.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Current page number (starts from 1).
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// Number of items returned per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of pages calculated based on <see cref="TotalCount"/> and <see cref="PageSize"/>.
        /// </summary>
        public int TotalPages { get; set; }
    }

}
