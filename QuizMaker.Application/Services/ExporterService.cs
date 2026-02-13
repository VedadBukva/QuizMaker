using System.Collections.Generic;
using System.Linq;
using QuizMaker.Domain.Export;

namespace QuizMaker.Application.Services
{
    /// <summary>
    /// Service responsible for providing information about available quiz exporters.
    /// </summary>
    /// <remarks>
    /// This service retrieves exporter metadata from the underlying exporter catalog.
    /// Exporters are dynamically discovered using MEF and exposed to clients
    /// so they can select a desired export format.
    /// </remarks>
    public class ExporterService
    {
        private readonly IExporterCatalog _catalog;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExporterService"/> class.
        /// </summary>
        /// <param name="catalog">
        /// Catalog used to retrieve dynamically registered exporters.
        /// </param>
        public ExporterService(IExporterCatalog catalog)
        {
            _catalog = catalog;
        }

        /// <summary>
        /// Retrieves all currently available exporters.
        /// </summary>
        /// <returns>
        /// A list of exporter metadata objects describing supported export formats.
        /// </returns>
        /// <remarks>
        /// The returned data includes exporter key, display name, file extension,
        /// and MIME type. The key must be used when invoking the quiz export endpoint.
        /// </remarks>
        public IList<ExporterDto> GetAll()
        {
            return _catalog.GetAll()
                .Select(x => new ExporterDto
                {
                    Key = x.Key,
                    DisplayName = x.DisplayName,
                    FileExtension = x.FileExtension,
                    MimeType = x.MimeType
                })
                .ToList();
        }
    }

}
