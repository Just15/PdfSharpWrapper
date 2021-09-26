using Microsoft.Extensions.Logging;
using PdfReaderWriter;
using PdfSharpCore.Pdf;
using System.Collections.Generic;

namespace PdfSharpWrapper
{
    public class PdfDocumentWriter : PdfDocumentBase
    {
        public PdfDocumentWriter(ILogger<PdfDocumentWriter> logger) : base(logger)
        {
            PdfHelpers.ExtendNet5EncodingSupport();
        }

        public bool Write(string filePath, Dictionary<string, string> dictionary)
        {
            return Write(filePath, dictionary, out _);
        }

        public bool Write(string filePath, Dictionary<string, string> dictionary, out Dictionary<string, string> unexpectedFieldTypes)
        {
            using (var pdfDocument = Open(filePath))
            {
                if (pdfDocument != null)
                {
                    return DoWrite(pdfDocument, dictionary, out unexpectedFieldTypes);
                }
                else
                {
                    unexpectedFieldTypes = null;
                    return false;
                }
            }
        }

        public bool TryWrite(string filePath, Dictionary<string, string> dictionary)
        {
            return TryWrite(filePath, dictionary, out _);
        }

        public bool TryWrite(string filePath, Dictionary<string, string> dictionary, out Dictionary<string, string> unexpectedFieldTypes)
        {
            using (var pdfDocument = TryOpen(filePath))
            {
                if (pdfDocument != null)
                {
                    return DoWrite(pdfDocument, dictionary, out unexpectedFieldTypes);
                }
                else
                {
                    unexpectedFieldTypes = null;
                    return false;
                }
            }
        }

        private bool DoWrite(PdfDocument pdfDocument, Dictionary<string, string> dictionary, out Dictionary<string, string> unexpectedFieldTypes)
        {
            unexpectedFieldTypes = new Dictionary<string, string>();
            var fields = pdfDocument.AcroForm.Fields;

            foreach (var keyValuePair in dictionary)
            {
                var field = fields[keyValuePair.Key];
                if (field == null)
                {
                    logger.LogError($"Field is null for key: {keyValuePair.Key}.");
                }
                else if (field.ReadOnly)
                {
                    logger.LogError($"'{field.Name}' is readonly.");
                }
                else
                {
                    //switch (field)
                    //{
                    //    case PdfTextField text:
                    //        var textPdfValue = dictionary[field.Name];
                    //        text.Value = new PdfString(textPdfValue);
                    //        break;
                    //    default:
                    //        break;
                    //}
                }
            }

            return true;
        }
    }
}
