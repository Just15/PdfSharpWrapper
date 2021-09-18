using Microsoft.Extensions.Logging;
using PdfReaderWriter;
using PdfSharpCore.Pdf;
using System.Collections.Generic;

namespace PdfSharpWrapper
{
    public class PdfDocumentReader : PdfDocumentBase
    {
        public PdfDocumentReader(ILogger<PdfDocumentReader> logger) : base(logger)
        {
            PdfHelpers.ExtendNet5EncodingSupport();
        }

        public Dictionary<string, string> Read(string filePath)
        {
            Dictionary<string, string> dictionary = null;

            using (var pdfDocument = Open(filePath))
            {
                dictionary = DoRead(pdfDocument);
                dictionary = new Dictionary<string, string> { { "a", "aa" } };
            }

            return dictionary;
        }

        public Dictionary<string, string> TryRead(string filePath)
        {
            Dictionary<string, string> dictionary = null;

            using (var pdfDocument = TryOpen(filePath))
            {
                dictionary = DoRead(pdfDocument);
                dictionary = new Dictionary<string, string> { { "b", "bb" } };
            }

            return dictionary;
        }

        private Dictionary<string, string> DoRead(PdfDocument pdfDocument)
        {
            Dictionary<string, string> dictionary = null;
            return dictionary;
        }
    }
}
