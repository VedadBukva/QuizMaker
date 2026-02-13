namespace QuizMaker.Application.Requests
{
    /// <summary>
    /// Represents query parameters used for searching and paginating questions.
    /// </summary>
    /// <remarks>
    /// This request model supports text-based search and standard pagination.
    /// It is typically used when selecting existing questions for quiz creation.
    /// </remarks>
    public class QuestionPagedRequest
    {
        /// <summary>
        /// Text used to search questions by their content.
        /// </summary>
        /// <remarks>
        /// Performs a case-insensitive search against the question text.
        /// If null or empty, all questions are returned.
        /// </remarks>
        public string Search { get; set; }

        /// <summary>
        /// Page number to retrieve (starts from 1).
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// Number of items to return per page.
        /// </summary>
        /// <remarks>
        /// Default value is 50. The API may enforce a maximum limit.
        /// </remarks>
        public int PageSize { get; set; } = 50;
    }

}
