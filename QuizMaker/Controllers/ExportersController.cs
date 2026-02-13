using System.Web.Http;
using QuizMaker.Application.Services;

namespace QuizMaker.Api.Controllers
{
    /// <summary>
    /// Provides access to available quiz export formats.
    /// </summary>
    /// <remarks>
    /// This controller exposes metadata about dynamically registered exporters.
    /// Exporters are discovered using MEF and can be used when calling the quiz export endpoint.
    /// </remarks>
    [RoutePrefix("api/exporters")]
    public class ExportersController : ApiController
    {
        private readonly ExporterService _exporterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportersController"/> class.
        /// </summary>
        /// <param name="exporterService">
        /// Service used to retrieve available exporter metadata.
        /// </param>
        public ExportersController(ExporterService exporterService)
        {
            _exporterService = exporterService;
        }

        /// <summary>
        /// Retrieves all currently available export formats.
        /// </summary>
        /// <returns>
        /// A collection of exporter metadata including key, display name, file extension and MIME type.
        /// </returns>
        /// <response code="200">Returns the list of available exporters.</response>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetAll()
        {
            var exporters = _exporterService.GetAll();
            return Ok(exporters);
        }
    }
}
