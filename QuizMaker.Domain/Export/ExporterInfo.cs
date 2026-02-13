namespace QuizMaker.Domain.Export
{
    /// <summary>
    /// Represents metadata describing a quiz exporter.
    /// </summary>
    /// <remarks>
    /// Contains descriptive information about an exporter implementation
    /// such as its unique key, display name, file extension and MIME type.
    /// 
    /// Instances of this class are typically created based on MEF metadata
    /// and exposed through the API to inform clients about available export formats.
    /// </remarks>
    public sealed class ExporterInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterInfo"/> class.
        /// </summary>
        /// <param name="key">
        /// Unique identifier of the exporter (e.g., "csv", "txt", "pdf").
        /// </param>
        /// <param name="displayName">
        /// Human-readable name of the exporter.
        /// </param>
        /// <param name="fileExtension">
        /// File extension associated with the exported format.
        /// </param>
        /// <param name="mimeType">
        /// MIME type used in HTTP responses for the exported file.
        /// </param>
        public ExporterInfo(string key, string displayName, string fileExtension, string mimeType)
        {
            Key = key;
            DisplayName = displayName;
            FileExtension = fileExtension;
            MimeType = mimeType;
        }

        /// <summary>
        /// Unique exporter key used when requesting export.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Display name shown to clients.
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// File extension of the exported file.
        /// </summary>
        public string FileExtension { get; }

        /// <summary>
        /// MIME type used in the HTTP response.
        /// </summary>
        public string MimeType { get; }
    }
}
