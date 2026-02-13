using QuizMaker.Domain.Entities;
using System;

namespace QuizMaker.Domain.Export
{
    /// <summary>
    /// Defines a contract for exporting quizzes into various file formats.
    /// </summary>
    /// <remarks>
    /// Implementations of this interface are responsible for generating
    /// file content for a given <see cref="Quiz"/> in a specific format
    /// (e.g., CSV, TXT, PDF).
    /// 
    /// Exporters are dynamically discovered using MEF and resolved
    /// through the <see cref="IExporterCatalog"/>.
    /// </remarks>
    public interface IQuizExporter
    {
        /// <summary>
        /// Generates an export file for the specified quiz.
        /// </summary>
        /// <param name="quiz">
        /// The quiz to export.
        /// </param>
        /// <returns>
        /// An <see cref="ExportResult"/> containing the file content,
        /// file name and MIME type.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if the provided quiz is null.
        /// </exception>
        ExportResult Export(Quiz quiz);
    }
}
