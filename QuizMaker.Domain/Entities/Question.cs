using QuizMaker.Domain.Entities.Helpers;
using System;
using System.Collections.Generic;

namespace QuizMaker.Domain.Entities
{
    /// <summary>
    /// Represents a quiz question stored in the system.
    /// </summary>
    /// <remarks>
    /// A question consists of textual content and a correct answer.
    /// 
    /// Questions can be reused across multiple quizzes through
    /// the <see cref="QuizQuestion"/> relationship entity.
    /// 
    /// Inherits audit and soft-delete properties from <see cref="BaseEntity"/>.
    /// </remarks>
    public class Question : BaseEntity
    {
        /// <summary>
        /// Unique identifier of the question.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Text content of the question.
        /// </summary>
        /// <remarks>
        /// This represents the visible question presented to participants.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Correct answer for the question.
        /// </summary>
        /// <remarks>
        /// Stored internally and not included in participant-facing exports.
        /// </remarks>
        public string CorrectAnswer { get; set; }

        /// <summary>
        /// Navigation property representing quizzes that include this question.
        /// </summary>
        /// <remarks>
        /// Implemented through a many-to-many relationship using the 
        /// <see cref="QuizQuestion"/> join entity.
        /// </remarks>
        public ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}