using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Export;

namespace QuizMaker.Exporters.Txt
{
    [Export(typeof(IQuizExporter))]
    [ExportMetadata("Key", "txt")]
    [ExportMetadata("DisplayName", "Plain Text")]
    [ExportMetadata("FileExtension", "txt")]
    [ExportMetadata("MimeType", "text/plain")]
    public sealed class TxtQuizExporter : IQuizExporter
    {
        public ExportResult Export(Quiz quiz)
        {
            var content = string.Join(
                Environment.NewLine,
                quiz.QuizQuestions
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => x.Question?.Text)
            );

            var utf8Bom = new byte[] { 0xEF, 0xBB, 0xBF };
            var contentBytes = Encoding.UTF8.GetBytes(content.ToString());
            var bytes = utf8Bom.Concat(contentBytes).ToArray();
            var safeName = MakeSafeFileName(quiz.Name);
            var fileName = $"{MakeSafeFileName(quiz.Name)}.txt";

            return new ExportResult(bytes, fileName, "text/plain");
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
