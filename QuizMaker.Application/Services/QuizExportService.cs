using QuizMaker.Application.Exceptions;
using QuizMaker.Domain.Export;
using QuizMaker.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace QuizMaker.Application.Services
{
    /// <summary>
    /// Service responsible for exporting quizzes to various file formats.
    /// </summary>
    /// <remarks>
    /// This service retrieves the requested quiz and delegates the export process
    /// to a dynamically discovered exporter implementation.
    /// Exporters are resolved via the exporter catalog (MEF-based).
    /// </remarks>
    public class QuizExportService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IExporterCatalog _catalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizExportService"/> class.
        /// </summary>
        /// <param name="quizRepository">
        /// Repository used to retrieve quiz data including associated questions.
        /// </param>
        /// <param name="catalog">
        /// Catalog responsible for resolving available exporters.
        /// </param>
        public QuizExportService(IQuizRepository quizRepository, IExporterCatalog catalog)
        {
            _quizRepository = quizRepository;
            _catalog = catalog;
        }

        /// <summary>
        /// Exports the specified quiz using the requested exporter.
        /// </summary>
        /// <param name="quizId">
        /// Unique identifier of the quiz to export.
        /// </param>
        /// <param name="exporterKey">
        /// Key identifying the exporter to use (e.g., "csv", "pdf", "txt").
        /// </param>
        /// <returns>
        /// An <see cref="ExportResult"/> containing file content, file name, and MIME type.
        /// </returns>
        /// <exception cref="MissingArgumentException">
        /// Thrown when <paramref name="quizId"/> is empty or <paramref name="exporterKey"/> is not provided.
        /// </exception>
        /// <exception cref="ExporterNotFoundException">
        /// Thrown when no exporter is registered for the specified key.
        /// </exception>
        /// <exception cref="EntityNotFoundException">
        /// Thrown when the quiz with the specified identifier does not exist.
        /// </exception>
        /// <remarks>
        /// The quiz is retrieved along with its associated questions.
        /// The exporter is responsible for generating the file content and metadata.
        /// </remarks>
        public async Task<ExportResult> ExportAsync(Guid quizId, string exporterKey)
        {
            if (quizId == Guid.Empty)
                throw new MissingArgumentException("Quiz id is required.", nameof(quizId));

            if (string.IsNullOrWhiteSpace(exporterKey))
                throw new MissingArgumentException("Exporter key is required.", nameof(exporterKey));

            var exporter = _catalog.GetByKey(exporterKey);
            if (exporter == null)
                throw new ExporterNotFoundException(exporterKey);

            var quiz = await _quizRepository.GetByIdWithQuestionsAsync(quizId);
            if (quiz == null)
                throw new EntityNotFoundException(nameof(quiz), quizId.ToString());

            return exporter.Export(quiz);
        }
    }

}
