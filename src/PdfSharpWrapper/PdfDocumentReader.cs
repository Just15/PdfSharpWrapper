using Microsoft.Extensions.Logging;
using PdfReaderWriter;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
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
            return Read(filePath, out _);
        }

        public Dictionary<string, string> Read(string filePath, out List<string> errorMessages)
        {
            Dictionary<string, string> dictionary = null;

            using (var pdfDocument = Open(filePath))
            {
                if (pdfDocument != null)
                {
                    dictionary = DoRead(pdfDocument, out errorMessages);
                }
                else
                {
                    errorMessages = null;
                }
            }

            return dictionary;
        }

        public Dictionary<string, string> TryRead(string filePath)
        {
            return TryRead(filePath, out _);
        }

        public Dictionary<string, string> TryRead(string filePath, out List<string> errorMessages)
        {
            Dictionary<string, string> dictionary = null;

            using (var pdfDocument = TryOpen(filePath))
            {
                if (pdfDocument != null)
                {
                    dictionary = DoRead(pdfDocument, out errorMessages);
                }
                else
                {
                    errorMessages = null;
                }
            }

            return dictionary;
        }

        private Dictionary<string, string> DoRead(PdfDocument pdfDocument, out List<string> errorMessages)
        {
            var dictionary = new Dictionary<string, string>();
            errorMessages = new List<string>();
            var fields = pdfDocument.AcroForm.Fields;

            foreach (var fieldName in fields.Names)
            {
                var field = fields[fieldName];
                switch (field)
                {
                    case PdfTextField text:
                        var textValue = !string.IsNullOrWhiteSpace(text.Text) ? text.Text : null;
                        dictionary.Add(fieldName, textValue);
                        break;
                    default:
                        logger.LogError($"Unexpected field type of '{field.GetType()}' for '{fieldName}'.");
                        errorMessages.Add($"The field '{fieldName}' is of unexpected type '{field.GetType()}'.");
                        break;
                }
            }

            return dictionary;
        }
    }
}
