using System;
using System.IO;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace PdfSharpWrapper
{
    public abstract class PdfDocumentBase
    {
        protected ILogger Logger { get; set; }

        protected PdfDocumentBase(ILogger logger)
        {
            Logger = logger;
        }

        public PdfDocument Open(string filePath)
        {
            PdfDocument pdfDocument;

            using (pdfDocument = PdfReader.Open(filePath))
            {
                if (pdfDocument.AcroForm == null)
                {
                    pdfDocument = null;
                    Logger.LogError($"'{nameof(PdfDocument.AcroForm)}' is null in '{filePath}'.");
                }
            }

            return pdfDocument;
        }

        public PdfDocument TryOpen(string filePath)
        {
            PdfDocument pdfDocument = null;

            if (File.Exists(filePath))
            {
                try
                {
                    pdfDocument = Open(filePath);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"Exception trying to open '{filePath}'.");
                }
            }
            else
            {
                Logger.LogError($"The file '{filePath}' does not exist.");
            }

            return pdfDocument;
        }
    }
}
