using System;

namespace QuizMaker.Application.Dtos
{
    /// <summary>
    /// Represents a lightweight view of a question.
    /// </summary>
    /// <remarks>
    /// This DTO is typically used in list or search results where 
    /// full question details are not required.
    /// </remarks>
    public class QuestionListItemDto
    {
        /// <summary>
        /// Unique identifier of the question.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Text content of the question.
        /// </summary>
        /// <remarks>
        /// Does not include the correct answer.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// UTC date and time when the question was created.
        /// </summary>
        /// <remarks>
        /// Stored in Coordinated Universal Time (UTC).
        /// </remarks>
        public DateTime CreatedAtUtc { get; set; }
    }

}
