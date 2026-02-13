using System;

namespace QuizMaker.Domain.Export
{
    /// <summary>
    /// Represents the result of a quiz export operation.
    /// </summary>
    /// <remarks>
    /// Encapsulates the generated file content along with its
    /// file name and MIME type, which are used to construct
    /// an HTTP response for file download.
    /// 
    /// Designed as an immutable value object.
    /// </remarks>
    public sealed class ExportResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportResult"/> class.
        /// </summary>
        /// <param name="content">
        /// Binary content of the exported file.
        /// If null, an empty byte array will be used.
        /// </param>
        /// <param name="fileName">
        /// Name of the exported file.
        /// Defaults to "export" if null or empty.
        /// </param>
        /// <param name="mimeType">
        /// MIME type of the exported file.
        /// Defaults to "application/octet-stream" if null or empty.
        /// </param>
        public ExportResult(byte[] content, string fileName, string mimeType)
        {
            Content = content ?? Array.Empty<byte>();
            FileName = string.IsNullOrWhiteSpace(fileName) ? "export" : fileName;
            MimeType = string.IsNullOrWhiteSpace(mimeType) ? "application/octet-stream" : mimeType;
        }

        /// <summary>
        /// Binary content of the exported file.
        /// </summary>
        public byte[] Content { get; }

        /// <summary>
        /// File name used in the Content-Disposition header.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// MIME type used in the HTTP response.
        /// </summary>
        public string MimeType { get; }
    }
}
