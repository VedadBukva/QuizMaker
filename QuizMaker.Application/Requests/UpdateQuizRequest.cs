using QuizMaker.Application.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuizMaker.Application.Requests
{
    /// <summary>
    /// Represents the request model used to update an existing quiz.
    /// </summary>
    /// <remarks>
    /// Allows updating the quiz name and redefining the set of questions
    /// that belong to the quiz.
    /// 
    /// The quiz can consist of:
    /// - Existing questions (referenced by their identifiers),
    /// - Newly created questions,
    /// - Or a combination of both.
    /// 
    /// Questions not included in the request will be removed from the quiz,
    /// but will remain stored in the system for future reuse.
    /// </remarks>
    public class UpdateQuizRequest
    {
        /// <summary>
        /// Updated name of the quiz.
        /// </summary>
        /// <remarks>
        /// If provided, replaces the existing quiz name.
        /// </remarks>
        [Required]
        [StringLength(ValidationConstants.QuizNameMaxLength, MinimumLength = ValidationConstants.QuizNameMinLength)]
        public string Name { get; set; }

        /// <summary>
        /// Collection of identifiers of existing questions to associate with the quiz.
        /// </summary>
        /// <remarks>
        /// Enables reusing previously created questions.
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
