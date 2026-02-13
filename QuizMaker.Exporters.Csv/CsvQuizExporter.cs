using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Export;

namespace QuizMaker.Exporters.Csv
{
    [Export(typeof(IQuizExporter))]
    [ExportMetadata("Key", "csv")]
    [ExportMetadata("DisplayName", "CSV")]
    [ExportMetadata("FileExtension", "csv")]
    [ExportMetadata("MimeType", "text/csv")]
    public sealed class CsvQuizExporter : IQuizExporter
    {
        public ExportResult Export(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            var sb = new StringBuilder();
            sb.AppendLine("Question");

            var questions = (quiz.QuizQuestions ?? Enumerable.Empty<QuizQuestion>())
                .OrderBy(x => x.DisplayOrder)
                .Select(x => x.Question?.Text ?? string.Empty);

            foreach (var q in questions)
                sb.AppendLine(EscapeCsv(q));

            var utf8Bom = new byte[] { 0xEF, 0xBB, 0xBF };
            var contentBytes = Encoding.UTF8.GetBytes(sb.ToString());
            var bytes = utf8Bom.Concat(contentBytes).ToArray();
            var safeName = MakeSafeFileName(quiz.Name);
            var fileName = $"{MakeSafeFileName(quiz.Name)}.csv";

            return new ExportResult(bytes, fileName, "text/csv");
        }

        private static string EscapeCsv(string value)
        {
            value = value ?? string.Empty;
            var needsQuotes = value.Contains(",") || value.Contains("\"") || value.Contains("\n") || value.Contains("\r");
            value = value.Replace("\"", "\"\"");

            return needsQuotes ? $"\"{value}\"" : value;
        }

        private static string MakeSafeFileName(string name)
        {
            name = string.IsNullOrWhiteSpace(name) ? "quiz" : name.Trim();

            name = name.Replace(' ', '_');

            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            while (name.Contains("__"))
                name = name.Replace("__", "_");

            return name.Trim('_', '.', ' ');
        }
    }
}
