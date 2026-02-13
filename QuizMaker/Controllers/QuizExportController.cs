using QuizMaker.Application.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace QuizMaker.Api.Controllers
{
    /// <summary>
    /// Provides functionality for exporting quizzes in various formats.
    /// </summary>
    /// <remarks>
    /// Allows clients to download a quiz in a selected file format (e.g., CSV, TXT, PDF).
    /// Available exporters are dynamically discovered using MEF.
    /// </remarks>
    [RoutePrefix("api/quizzes")]
    public class QuizExportController : ApiController
    {
        private readonly QuizExportService _quizExportService;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizExportController"/> class.
        /// </summary>
        /// <param name="quizExportService">
        /// Service responsible for handling quiz export operations.
        /// </param>
        public QuizExportController(QuizExportService quizExportService)
        {
            _quizExportService = quizExportService;
        }

        /// <summary>
        /// Exports a quiz in the specified format.
        /// </summary>
        /// <param name="id">
        /// Unique identifier of the quiz to export.
        /// </param>
        /// <param name="exporter">
        /// Key of the exporter to use (e.g., "csv", "txt", "pdf").
        /// </param>
        /// <returns>
        /// A downloadable file containing quiz questions (without answers).
        /// </returns>
        /// <response code="200">Returns the exported file.</response>
        /// <response code="400">Exporter key is missing or invalid.</response>
        /// <response code="404">Quiz not found.</response>
        /// <remarks>
        /// The exporter key must match one of the keys returned by the 
        /// <c>GET /api/exporters</c> endpoint.
        /// 
        /// The response includes:
        /// - File content (binary)
        /// - Proper MIME type
        /// - Content-Disposition header for file download
        /// </remarks>
        [HttpGet]
        [Route("{id:guid}/export")]
        public async Task<HttpResponseMessage> Export(Guid id, [FromUri] string exporter)
        {
            var result = await _quizExportService.ExportAsync(id, exporter);

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(result.Content);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(result.MimeType);

            var cd = new ContentDispositionHeaderValue("attachment")
            {
                FileName = result.FileName,
                FileNameStar = result.FileName
            };

            response.Content.Headers.ContentDisposition = cd;

            return response;
        }
    }

}
