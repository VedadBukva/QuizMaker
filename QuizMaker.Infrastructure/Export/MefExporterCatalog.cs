using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using QuizMaker.Domain.Export;

namespace QuizMaker.Infrastructure.Export
{
    /// <inheritdoc />
    public sealed class MefExporterCatalog : IExporterCatalog, IDisposable
    {
        private readonly CompositionContainer _container;
        private readonly IList<Lazy<IQuizExporter, IQuizExporterMetadata>> _exporters;

        /// <summary>
        /// MefExporterCatalog constructor.
        /// </summary>
        /// <param name="exportersFolderPath"></param>
        /// <exception cref="ArgumentException"></exception>
        public MefExporterCatalog(string exportersFolderPath)
        {
            if (string.IsNullOrWhiteSpace(exportersFolderPath))
                throw new ArgumentException("Exporters folder path is required.", nameof(exportersFolderPath));

            var aggregate = new AggregateCatalog();

            aggregate.Catalogs.Add(new DirectoryCatalog(exportersFolderPath, "QuizMaker.Exporters.*.dll"));

            _container = new CompositionContainer(aggregate);

            _exporters = _container
                .GetExports<IQuizExporter, IQuizExporterMetadata>()
                .ToList()
                .AsReadOnly();
        }

        /// <inheritdoc />
        public IList<ExporterInfo> GetAll()
        {
            return _exporters
                .Select(e => new ExporterInfo(
                    e.Metadata.Key,
                    e.Metadata.DisplayName,
                    e.Metadata.FileExtension,
                    e.Metadata.MimeType))
                .OrderBy(x => x.DisplayName)
                .ToList();
        }

        /// <inheritdoc />
        public IQuizExporter GetByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var match = _exporters.FirstOrDefault(e =>
                string.Equals(e.Metadata.Key, key, StringComparison.OrdinalIgnoreCase));

            return match?.Value;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}
