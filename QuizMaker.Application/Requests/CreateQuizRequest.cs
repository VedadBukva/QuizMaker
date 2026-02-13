using QuizMaker.Application.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizMaker.Application.Requests
{
    /// <summary>
    /// Represents the request model used to create a new quiz.
    /// </summary>
    /// <remarks>
    /// A quiz can consist of:
    /// - Existing questions (referenced by their identifiers),
    /// - Newly created questions,
    /// - Or a combination of both.
    /// </remarks>
    public class CreateQuizRequest
    {
        /// <summary>
        /// Name of the quiz.
        /// </summary>
        /// <remarks>
        /// This value is required and should be unique within the system.
        /// </remarks>
        [Required]
        [StringLength(ValidationConstants.QuizNameMaxLength, MinimumLength = ValidationConstants.QuizNameMinLength)]
        public string Name { get; set; }

        /// <summary>
        /// Collection of identifiers of existing questions to be reused in the new quiz.
        /// </summary>
        /// <remarks>
        /// This enables "question recycling", allowing previously created questions
        /// to be included in the new quiz.
        /// </remarks>
        public IList<Guid> ExistingQuestionIds { get; set; } = new List<Guid>();

        /// <summary>
        /// Collection of new questions to be created and added to the quiz.
        /// </summary>
        /// <remarks>
        /// Each question must include question text and correct answer.
        /// </remarks>
        [MaxLength(ValidationConstants.QuizQuestionsMaxLength)]
        public IList<NewQuestionRequest> NewQuestions { get; set; } = new List<NewQuestionRequest>();
    }
}
