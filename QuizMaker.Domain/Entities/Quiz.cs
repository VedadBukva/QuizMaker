using QuizMaker.Domain.Entities.Helpers;
using System;
using System.Collections.Generic;

namespace QuizMaker.Domain.Entities
{
    /// <summary>
    /// Represents a quiz consisting of multiple questions.
    /// </summary>
    /// <remarks>
    /// A quiz is identified by a unique name and contains an ordered collection
    /// of questions defined through the <see cref="QuizQuestion"/> join entity.
    /// 
    /// Questions can be reused across multiple quizzes.
    /// 
    /// Inherits audit and soft-delete properties from <see cref="BaseEntity"/>.
    /// </remarks>
    public class Quiz : BaseEntity
    {
        /// <summary>
        /// Unique identifier of the quiz.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the quiz.
        /// </summary>
        /// <remarks>
        /// Defined during quiz creation and can be updated later.
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Navigation property representing questions included in the quiz.
        /// </summary>
        /// <remarks>
        /// The relationship is managed via the <see cref="QuizQuestion"/> entity,
        /// which preserves question order using a display index.
        /// </remarks>
        public ICollection<QuizQuestion> QuizQuestions { get; set; }
    }
}
