using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Export;

namespace QuizMaker.Exporters.Xml
{
    [Export(typeof(IQuizExporter))]
    [ExportMetadata("Key", "xml")]
    [ExportMetadata("DisplayName", "XML")]
    [ExportMetadata("FileExtension", "xml")]
    [ExportMetadata("MimeType", "application/xml")]
    public sealed class XmlQuizExporter : IQuizExporter
    {
        public ExportResult Export(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            var document =
                new XDocument(
                    new XElement("Quiz",
                        new XAttribute("Name", quiz.Name),
                        (quiz.QuizQuestions ?? Enumerable.Empty<QuizQuestion>())
                            .OrderBy(x => x.DisplayOrder)
                            .Select(x =>
                                new XElement("Question",
                                    new XElement("Text", x.Question?.Text)
                                )
                            )
                    )
                );

            var xmlString = document.ToString();
            var bytes = Encoding.UTF8.GetBytes(xmlString);

            var fileName = $"{MakeSafeFileName(quiz.Name)}.xml";

            return new ExportResult(bytes, fileName, "application/xml");
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
