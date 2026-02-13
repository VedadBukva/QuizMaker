namespace QuizMaker.Domain.Export
{
    /// <summary>
    /// Defines metadata associated with a quiz exporter implementation.
    /// </summary>
    /// <remarks>
    /// Used by MEF (Managed Extensibility Framework) to describe
    /// exporter characteristics without instantiating the exporter itself.
    /// 
    /// This metadata is typically applied via <c>ExportMetadata</c>
    /// attributes and consumed by <see cref="IExporterCatalog"/>.
    /// </remarks>
    public interface IQuizExporterMetadata
    {
        /// <summary>
        /// Unique key identifying the exporter.
        /// </summary>
        /// <remarks>
        /// Used by clients when requesting export (e.g., "csv", "txt", "pdf").
        /// </remarks>
        string Key { get; }

        /// <summary>
        /// Human-readable display name of the exporter.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// File extension associated with the exported format.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// MIME type used when returning the exported file.
        /// </summary>
        string MimeType { get; }
    }
}
