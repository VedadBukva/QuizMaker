namespace QuizMaker.Application.Requests
{
    /// <summary>
    /// Represents a request model for creating a new question.
    /// </summary>
    /// <remarks>
    /// This model is used when adding new questions while creating or updating a quiz.
    /// Both the question text and the correct answer are required.
    /// </remarks>
    public class NewQuestionRequest
    {
        /// <summary>
        /// Text content of the question.
        /// </summary>
        /// <remarks>
        /// Must not be empty. The text should clearly describe the question.
        /// </remarks>
        public string Text { get; set; }

        /// <summary>
        /// Correct answer to the question.
        /// </summary>
        /// <remarks>
        /// This value is stored internally and is not included in participant exports.
        /// </remarks>
        public string CorrectAnswer { get; set; }
    }
}
