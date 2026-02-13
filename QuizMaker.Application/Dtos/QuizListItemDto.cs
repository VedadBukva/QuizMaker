using System;

namespace QuizMaker.Application.Dtos
{
    /// <summary>
    /// Represents a lightweight view of a quiz used in list responses.
    /// </summary>
    /// <remarks>
    /// This DTO is typically returned when retrieving a paged list of quizzes.
    /// It does not include full question details.
    /// </remarks>
    public class QuizListItemDto
    {
        /// <summary>
        /// Unique identifier of the quiz.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the quiz.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Total number of questions associated with the quiz.
        /// </summary>
        public int QuestionCount { get; set; }

        /// <summary>
        /// UTC date and time when the quiz was created.
        /// </summary>
        /// <remarks>
        /// Stored in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime CreatedAtUtc { get; set; }
    }

}
