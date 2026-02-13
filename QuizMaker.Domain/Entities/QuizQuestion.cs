using QuizMaker.Domain.Entities.Helpers;
using System;

namespace QuizMaker.Domain.Entities
{
    /// <summary>
    /// Represents the relationship between a <see cref="Quiz"/> and a <see cref="Question"/>.
    /// </summary>
    /// <remarks>
    /// This entity acts as a join table in a many-to-many relationship
    /// while also preserving the display order of questions within a quiz.
    /// 
    /// Each record defines:
    /// - Which question belongs to which quiz
    /// - The order in which the question appears
    /// 
    /// Inherits audit and soft-delete properties from <see cref="BaseEntity"/>.
    /// </remarks>
    public class QuizQuestion : BaseEntity
    {
        /// <summary>
        /// Identifier of the quiz.
        /// </summary>
        public Guid QuizId { get; set; }

        /// <summary>
        /// Identifier of the associated question.
        /// </summary>
        public Guid QuestionId { get; set; }

        /// <summary>
        /// Navigation property to the related quiz.
        /// </summary>
        public Quiz Quiz { get; set; }

        /// <summary>
        /// Navigation property to the related question.
        /// </summary>
        public Question Question { get; set; }

        /// <summary>
        /// Determines the position of the question within the quiz.
        /// </summary>
        /// <remarks>
        /// Questions are displayed in ascending order based on this value.
        /// </remarks>
        public int DisplayOrder { get; set; }
    }
}