/// <summary>
/// Represents metadata describing an available quiz exporter.
/// </summary>
/// <remarks>
/// This DTO is returned by the API to inform clients which export formats 
/// are currently supported by the system.
/// </remarks>
public class ExporterDto
{
    /// <summary>
    /// Unique identifier of the exporter.
    /// </summary>
    /// <remarks>
    /// This value must be provided as the <c>exporter</c> query parameter 
    /// when calling the quiz export endpoint.
    /// Example: "csv", "pdf", "txt".
    /// </remarks>
    public string Key { get; set; }

    /// <summary>
    /// Human-readable name of the exporter.
    /// </summary>
    /// <remarks>
    /// Intended for display purposes in the UI.
    /// Example: "CSV", "PDF", "Plain Text".
    /// </remarks>
    public string DisplayName { get; set; }

    /// <summary>
    /// File extension produced by the exporter.
    /// </summary>
    /// <remarks>
    /// Does not include the leading dot.
    /// Example: "csv", "pdf", "txt".
    /// </remarks>
    public string FileExtension { get; set; }

    /// <summary>
    /// MIME type associated with the exported file.
    /// </summary>
    /// <remarks>
    /// Used to correctly set the <c>Content-Type</c> header in the response.
    /// Example: "text/csv", "application/pdf".
    /// </remarks>
    public string MimeType { get; set; }
}
