using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using QuizMaker.Domain.Entities;
using QuizMaker.Domain.Export;

namespace QuizMaker.Exporters.Pdf
{
    [Export(typeof(IQuizExporter))]
    [ExportMetadata("Key", "pdf")]
    [ExportMetadata("DisplayName", "PDF")]
    [ExportMetadata("FileExtension", "pdf")]
    [ExportMetadata("MimeType", "application/pdf")]
    public sealed class PdfQuizExporter : IQuizExporter
    {
        public ExportResult Export(Quiz quiz)
        {
            if (quiz == null) throw new ArgumentNullException(nameof(quiz));

            var doc = new PdfDocument();
            var page = doc.AddPage();
            page.Size = PdfSharp.PageSize.A4;

            using (var gfx = XGraphics.FromPdfPage(page))
            {
                var titleFont = new XFont("Arial", 16, XFontStyleEx.Regular);
                var textFont = new XFont("Arial", 11, XFontStyleEx.Regular);

                double x = 40;
                double y = 50;

                gfx.DrawString(quiz.Name ?? "Quiz", titleFont, XBrushes.Black, new XPoint(x, y));
                y += 30;

                var questions = (quiz.QuizQuestions ?? Enumerable.Empty<QuizQuestion>())
                    .OrderBy(q => q.DisplayOrder)
                    .Select(q => q.Question?.Text ?? string.Empty)
                    .ToList();

                int i = 1;
                foreach (var q in questions)
                {
                    gfx.DrawString($"{i}. {q}", textFont, XBrushes.Black, new XPoint(x, y));
                    y += 18;
                    i++;

                    if (y > page.Height - 40)
                        break;
                }
            }

            byte[] bytes;
            using (var ms = new MemoryStream())
            {
                doc.Save(ms);
                bytes = ms.ToArray();
            }

            var fileName = $"{MakeSafeFileName(quiz.Name)}.pdf";
            return new ExportResult(bytes, fileName, "application/pdf");
        }

        private static string MakeSafeFileName(string name)
        {
            name = string.IsNullOrWhiteSpace(name) ? "quiz" : name.Trim();
            name = name.Replace(' ', '_');

            foreach (var c in Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');

            while (name.Contains("__"))
                name = name.Replace("__", "_");

            return name.Trim('_', '.', ' ');
        }
    }
}
