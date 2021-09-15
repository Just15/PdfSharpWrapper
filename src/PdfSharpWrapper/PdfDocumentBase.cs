using Microsoft.Extensions.Logging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.IO;

namespace PdfSharpWrapper
{
    public class PdfDocumentBase
    {
        protected readonly ILogger logger;

        public PdfDocumentBase(ILogger logger)
        {
            this.logger = logger;
        }

        public PdfDocument Open(string pdfFilePath)
        {
            PdfDocument pdfDocument;

            using (pdfDocument = PdfReader.Open(pdfFilePath))
            {
                if (pdfDocument.AcroForm == null)
                {
                    pdfDocument = null;
                    logger.LogError($"'{nameof(PdfDocument.AcroForm)}' is null in '{pdfFilePath}'.");
                }
            }

            return pdfDocument;
        }

        public PdfDocument TryOpen(string pdfFilePath)
        {
            PdfDocument pdfDocument = null;

            if (File.Exists(pdfFilePath))
            {
                try
                {
                    pdfDocument = Open(pdfFilePath);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Exception trying to open '{pdfFilePath}'.");
                }

            }
            else
            {
                logger.LogError($"The file '{pdfFilePath}' does not exist.");
            }

            return pdfDocument;
        }
    }
}
