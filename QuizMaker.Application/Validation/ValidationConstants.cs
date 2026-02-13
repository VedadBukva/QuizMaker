namespace QuizMaker.Application.Validation
{
    /// <summary>
    /// Defines validation-related constants used across the application.
    /// </summary>
    /// <remarks>
    /// Centralizes maximum lengths and paging constraints to ensure
    /// consistency between:
    /// - Domain validation
    /// - Data annotations
    /// - Database configuration
    /// - Service-layer validation
    /// 
    /// Prevents magic numbers and promotes maintainability.
    /// </remarks>
    public static class ValidationConstants
    {
        /// <summary>
        /// Maximum allowed length for a quiz name.
        /// </summary>
        /// <remarks>
        /// Used in request validation and database configuration.
        /// </remarks>
        public const int QuizNameMaxLength = 200;

        /// <summary>
        /// Minimum allowed length for a quiz name.
        /// </summary>
        /// <remarks>
        /// Used in request validation and database configuration.
        /// </remarks>
        public const int QuizNameMinLength = 3;

        /// <summary>
        /// Maximum allowed length for a quiz questions.
        /// </summary>
        /// <remarks>
        /// Used in request validation and database configuration.
        /// </remarks>
        public const int QuizQuestionsMaxLength = 200;

        /// <summary>
        /// Maximum allowed length for question text.
        /// </summary>
        /// <remarks>
        /// Ensures question content does not exceed database limits.
        /// </remarks>
        public const int QuestionTextMaxLength = 1000;

        /// <summary>
        /// Minimum allowed length for question text.
        /// </summary>
        /// <remarks>
        /// Ensures question content does not exceed database limits.
        /// </remarks>
        public const int QuestionTextMinLength = 10;

        /// <summary>
        /// Maximum allowed length for the correct answer of a question.
        /// </summary>
        /// <remarks>
        /// Applied during validation and persistence.
        /// </remarks>
        public const int CorrectAnswerMaxLength = 1000;

        /// <summary>
        /// Default page number.
        /// </summary>
        public const int DefaultPage = 1;

        /// <summary>
        /// Default number of items returned per page when paging parameters are not provided.
        /// </summary>
        public const int DefaultPageSize = 50;

        /// <summary>
        /// Maximum allowed number of items per page.
        /// </summary>
        /// <remarks>
        /// Prevents excessive data retrieval and protects API performance.
        /// </remarks>
        public const int MaxPageSize = 200;
    }

}
