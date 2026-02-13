using Newtonsoft.Json;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Export;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml;

namespace QuizMaker.Exporters.Json
{
    [Export(typeof(IQuizExporter))]
    [ExportMetadata("Key", "json")]
    [ExportMetadata("DisplayName", "JSON")]
    [ExportMetadata("FileExtension", "json")]
    [ExportMetadata("MimeType", "application/json")]
    public sealed class JsonQuizExporter : IQuizExporter
    {
        public ExportResult Export(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            var data = new
            {
                quiz.Name,
                Questions = (quiz.QuizQuestions ?? Enumerable.Empty<QuizQuestion>())
                    .OrderBy(x => x.DisplayOrder)
                    .Select(x => new
                    {
                        Text = x.Question?.Text
                    })
            };

            var json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            var bytes = Encoding.UTF8.GetBytes(json);

            var fileName = $"{MakeSafeFileName(quiz.Name)}.json";

            return new ExportResult(bytes, fileName, "application/json");
        }

        private static string MakeSafeFileName(string name)
        {
            name = string.IsNullOrWhiteSpace(name) ? "quiz" : name.Trim();
            foreach (var c in System.IO.Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name.Replace(' ', '_');
        }
    }
}
