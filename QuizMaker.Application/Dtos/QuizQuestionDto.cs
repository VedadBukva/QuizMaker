using System;

namespace QuizMaker.Application.Dtos
{
    /// <summary>
    /// Represents a question that belongs to a quiz.
    /// </summary>
    /// <remarks>
    /// This DTO contains both the question text and its correct answer.
    /// It is typically returned when retrieving detailed quiz information.
    /// </remarks>
    public class QuizQuestionDto
    {
        /// <summary>
        /// Unique identifier of the question.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Text content of the question.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Correct answer for the question.
        /// </summary>
        /// <remarks>
        /// This value is used for quiz validation purposes and is not included in export formats intended for participants.
        /// </remarks>
        public string CorrectAnswer { get; set; }
    }
}
