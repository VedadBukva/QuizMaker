namespace QuizMaker.Domain.Enums
{
    /// <summary>
    /// Defines sorting direction for paginated queries.
    /// </summary>
    /// <remarks>
    /// Used when retrieving paged results to determine
    /// whether data should be sorted in ascending or descending order.
    /// </remarks>
    public enum SortOrder
    {
        /// <summary>
        /// Sort results in ascending order.
        /// </summary>
        Asc = 0,

        /// <summary>
        /// Sort results in descending order.
        /// </summary>
        Desc = 1
    }

}
