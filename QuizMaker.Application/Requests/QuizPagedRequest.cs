using QuizMaker.Domain.Enums;

namespace QuizMaker.Application.Requests
{
    /// <summary>
    /// Represents query parameters used for searching and paginating quizzes.
    /// </summary>
    /// <remarks>
    /// This request model supports text-based search and standard pagination.
    /// </remarks>
    public class QuizPagedRequest
    {
        /// <summary>
        /// Text used to search quiz by name.
        /// </summary>
        public string Search { get; set; }

        /// <summary>
        /// Page number (starts from 1).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items per page (default 50).
        /// </summary>
        public int PageSize { get; set; } = 50;

        /// <summary>
        /// Defines sorting direction by quiz name. Allowed values: Asc or Desc.
        /// Default: Desc.
        /// </summary>
        public SortOrder SortOrder { get; set; } = SortOrder.Desc;
    }
}
