using System;

namespace QuizMaker.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a required argument is missing.
    /// </summary>
    /// <remarks>
    /// Typically mapped to an HTTP 400 (Bad Request) response.
    /// </remarks>
    public sealed class MissingArgumentException : ArgumentException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingArgumentException"/> class.
        /// </summary>
        /// <param name="message">Description of the missing argument error.</param>
        /// <param name="paramName">Name of the missing parameter.</param>
        public MissingArgumentException(string message, string paramName)
            : base(message, paramName)
        {
        }
    }

}
