using System;
using System.Collections.Generic;

namespace QuizMaker.Application.Dtos
{
    /// <summary>
    /// Represents detailed information about a quiz.
    /// </summary>
    /// <remarks>
    /// This DTO is returned when retrieving a single quiz,
    /// including all associated questions.
    /// </remarks>
    public class QuizDetailsDto
    {
        /// <summary>
        /// Unique identifier of the quiz.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the quiz.
        /// </summary>
        /// <remarks>
        /// The name is defined by the user during quiz creation or update.
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Collection of questions that belong to the quiz.
        /// </summary>
        /// <remarks>
        /// Questions are typically returned in the defined display order.
        /// </remarks>
        public IList<QuizQuestionDto> Questions { get; set; }
    }

}
