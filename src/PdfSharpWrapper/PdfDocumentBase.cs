using System;
using System.IO;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;

namespace PdfSharpWrapper
{
    public abstract class PdfDocumentBase
    {
        protected readonly ILogger logger;

        protected PdfDocumentBase(ILogger logger)
        {
            this.logger = logger;
        }

        public PdfDocument Open(string filePath)
        {
            PdfDocument pdfDocument;

            using (pdfDocument = PdfReader.Open(filePath))
            {
                if (pdfDocument.AcroForm == null)
                {
                    pdfDocument = null;
                    logger.LogError($"'{nameof(PdfDocument.AcroForm)}' is null in '{filePath}'.");
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
                    logger.LogError(ex, $"Exception trying to open '{filePath}'.");
                }
            }
            else
            {
                logger.LogError($"The file '{filePath}' does not exist.");
            }

            return pdfDocument;
        }
    }
}
