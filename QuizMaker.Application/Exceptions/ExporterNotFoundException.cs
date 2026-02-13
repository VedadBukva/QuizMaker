using System;

namespace QuizMaker.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested exporter cannot be found.
    /// </summary>
    /// <remarks>
    /// This exception is typically raised when the provided exporter key
    /// does not match any registered exporter implementation.
    /// It is commonly mapped to an HTTP 400 (Bad Request) or 404 (Not Found) response,
    /// depending on API design decisions.
    /// </remarks>
    public sealed class ExporterNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterNotFoundException"/> class.
        /// </summary>
        /// <param name="key">The exporter key that was requested.</param>
        public ExporterNotFoundException(string key)
            : base("Exporter not found.")
        {
            Key = key;
        }

        /// <summary>
        /// Gets the exporter key that could not be resolved.
        /// </summary>
        public string Key { get; }
    }

}
