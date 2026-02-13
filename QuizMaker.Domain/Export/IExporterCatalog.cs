using System.Collections.Generic;

namespace QuizMaker.Domain.Export
{
    /// <summary>
    /// Provides access to available quiz exporters.
    /// </summary>
    /// <remarks>
    /// Responsible for discovering and exposing exporter implementations,
    /// typically loaded dynamically using MEF (Managed Extensibility Framework).
    /// 
    /// Allows:
    /// - Retrieving metadata for all available exporters
    /// - Resolving a specific exporter by its unique key
    /// </remarks>
    public interface IExporterCatalog
    {
        /// <summary>
        /// Retrieves metadata for all available exporters.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="ExporterInfo"/> describing
        /// all dynamically loaded exporters.
        /// </returns>
        IList<ExporterInfo> GetAll();

        /// <summary>
        /// Retrieves an exporter implementation by its unique key.
        /// </summary>
        /// <param name="key">
        /// Unique exporter identifier (e.g., "csv", "txt", "pdf").
        /// </param>
        /// <returns>
        /// An instance of <see cref="IQuizExporter"/> if found;
        /// otherwise null.
        /// </returns>
        IQuizExporter GetByKey(string key);
    }

}
